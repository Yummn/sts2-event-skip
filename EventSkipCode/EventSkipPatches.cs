using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using HarmonyLib;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Events;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Events.Custom;

namespace EventSkip;

internal sealed class EventSkipState
{
    internal bool ChoiceCommitted;
    internal bool Skipping;
}

internal static class EventSkipRuntime
{
    internal const string SkipTextKey = "EVENT_SKIP";

    private static readonly ConditionalWeakTable<EventModel, EventSkipState> EventStates = new();
    private static readonly ConditionalWeakTable<EventOption, EventModel> OptionOwners = new();
    private static readonly MethodInfo SetEventFinishedMethod =
        AccessTools.Method(typeof(EventModel), "SetEventFinished")
        ?? throw new MissingMethodException(typeof(EventModel).FullName, "SetEventFinished");
    private static readonly MethodInfo AncientDoneMethod =
        AccessTools.Method(typeof(AncientEventModel), "Done")
        ?? throw new MissingMethodException(typeof(AncientEventModel).FullName, "Done");
    private static readonly object LocalizationLock = new();
    private static string? _localizedLanguage;

    internal static void AddSkipOption(EventModel eventModel, ref IEnumerable<EventOption> eventOptions)
    {
        EnsureLocalization();

        var options = eventOptions as List<EventOption> ?? eventOptions.ToList();
        foreach (EventOption option in options)
        {
            RememberOwner(option, eventModel);
        }

        EventSkipState state = EventStates.GetOrCreateValue(eventModel);
        if (state.Skipping ||
            state.ChoiceCommitted ||
            eventModel.IsFinished ||
            options.Count == 0 ||
            options.Any(option => option.TextKey == SkipTextKey))
        {
            eventOptions = options;
            return;
        }

        EventOption skip = CreateSkipOption(eventModel);
        options.Add(skip);
        RememberOwner(skip, eventModel);
        eventOptions = options;
        MainFile.Logger.Info($"[EventSkip] added Skip to {eventModel.Id.Entry}; reward={GetReward(eventModel)}.");
    }

    internal static void MarkNormalChoiceCommitted(EventOption option)
    {
        if (option.TextKey == SkipTextKey || option.WasChosen)
        {
            return;
        }

        if (OptionOwners.TryGetValue(option, out EventModel? eventModel))
        {
            EventStates.GetOrCreateValue(eventModel).ChoiceCommitted = true;
        }
    }

    internal static void EnsureLocalization()
    {
        try
        {
            LocManager manager = LocManager.Instance;
            string language = manager.Language ?? "eng";

            lock (LocalizationLock)
            {
                if (_localizedLanguage == language)
                {
                    return;
                }

                bool chinese = language.StartsWith("zh", StringComparison.OrdinalIgnoreCase) ||
                               language.Equals("zhs", StringComparison.OrdinalIgnoreCase) ||
                               language.Equals("zht", StringComparison.OrdinalIgnoreCase);

                Dictionary<string, string> strings = chinese
                    ? new Dictionary<string, string>
                    {
                        ["EVENT_SKIP.title"] = "跳过",
                        ["EVENT_SKIP.description"] = "离开，不触发这个事件的任何选项。",
                        ["EVENT_SKIP.ancient.description"] = "跳过先古之民，获得200金币。",
                        ["EVENT_SKIP.neow.description"] = "跳过涅奥，获得100金币。",
                        ["EVENT_SKIP.done"] = "你没有停留，继续前进。"
                    }
                    : new Dictionary<string, string>
                    {
                        ["EVENT_SKIP.title"] = "Skip",
                        ["EVENT_SKIP.description"] = "Leave without choosing any of this event's options.",
                        ["EVENT_SKIP.ancient.description"] = "Skip this Ancient and gain 200 Gold.",
                        ["EVENT_SKIP.neow.description"] = "Skip Neow and gain 100 Gold.",
                        ["EVENT_SKIP.done"] = "You move on without stopping."
                    };

                manager.GetTable("events").MergeWith(strings);
                _localizedLanguage = language;
            }
        }
        catch (Exception exception)
        {
            MainFile.Logger.Error($"[EventSkip] localization injection failed: {exception}");
        }
    }

    private static EventOption CreateSkipOption(EventModel eventModel)
    {
        string descriptionKey = eventModel is Neow
            ? "EVENT_SKIP.neow.description"
            : IsAncient(eventModel)
                ? "EVENT_SKIP.ancient.description"
                : "EVENT_SKIP.description";

        return new EventOption(
            eventModel,
            () => SkipAsync(eventModel),
            new LocString("events", "EVENT_SKIP.title"),
            new LocString("events", descriptionKey),
            SkipTextKey,
            Array.Empty<IHoverTip>());
    }

    private static async Task SkipAsync(EventModel eventModel)
    {
        EventSkipState state = EventStates.GetOrCreateValue(eventModel);
        if (state.Skipping)
        {
            return;
        }

        state.Skipping = true;
        state.ChoiceCommitted = true;

        try
        {
            int reward = GetReward(eventModel);
            if (reward > 0)
            {
                var owner = eventModel.Owner
                    ?? throw new InvalidOperationException("Cannot skip an event before it has an owner.");
                await PlayerCmd.GainGold(reward, owner);
            }

            if (eventModel is AncientEventModel ancient)
            {
                AncientDoneMethod.Invoke(ancient, null);
            }
            else
            {
                SetEventFinishedMethod.Invoke(
                    eventModel,
                    new object[] { new LocString("events", "EVENT_SKIP.done") });
            }

            MainFile.Logger.Info($"[EventSkip] skipped {eventModel.Id.Entry}; gained {reward} gold.");
        }
        catch (Exception exception)
        {
            state.Skipping = false;
            MainFile.Logger.Error($"[EventSkip] failed while skipping {eventModel.Id.Entry}: {exception}");
            throw;
        }
    }

    private static int GetReward(EventModel eventModel)
    {
        if (eventModel is Neow)
        {
            return 100;
        }

        return IsAncient(eventModel) ? 200 : 0;
    }

    private static bool IsAncient(EventModel eventModel)
    {
        return eventModel is AncientEventModel ||
               string.Equals(eventModel.LocTable, "ancients", StringComparison.OrdinalIgnoreCase);
    }

    private static void RememberOwner(EventOption option, EventModel eventModel)
    {
        OptionOwners.Remove(option);
        OptionOwners.Add(option, eventModel);
    }
}

[HarmonyPatch]
internal static class EventModelSetEventStatePatch
{
    private static MethodBase TargetMethod()
    {
        return AccessTools.Method(
                   typeof(EventModel),
                   "SetEventState",
                   new[] { typeof(LocString), typeof(IEnumerable<EventOption>) })
               ?? throw new MissingMethodException(typeof(EventModel).FullName, "SetEventState");
    }

    private static void Prefix(EventModel __instance, ref IEnumerable<EventOption> eventOptions)
    {
        EventSkipRuntime.AddSkipOption(__instance, ref eventOptions);
    }
}

[HarmonyPatch(typeof(EventOption), nameof(EventOption.Chosen))]
internal static class EventOptionChosenPatch
{
    private static void Prefix(EventOption __instance)
    {
        EventSkipRuntime.MarkNormalChoiceCommitted(__instance);
    }
}

[HarmonyPatch(typeof(NFakeMerchant), nameof(NFakeMerchant._Ready))]
internal static class FakeMerchantReadyPatch
{
    private static readonly FieldInfo ProceedButtonField =
        AccessTools.Field(typeof(NFakeMerchant), "_proceedButton")
        ?? throw new MissingFieldException(typeof(NFakeMerchant).FullName, "_proceedButton");

    private static void Postfix(NFakeMerchant __instance)
    {
        EventSkipRuntime.EnsureLocalization();
        if (ProceedButtonField.GetValue(__instance) is NProceedButton button)
        {
            button.UpdateText(new LocString("events", "EVENT_SKIP.title"));
        }
    }
}
