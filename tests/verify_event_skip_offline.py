from pathlib import Path


ROOT = Path(__file__).resolve().parents[1]
SOURCE = (ROOT / "EventSkipCode" / "EventSkipPatches.cs").read_text(encoding="utf-8")
MANIFEST = (ROOT / "EventSkip.json").read_text(encoding="utf-8")
PROJECT = (ROOT / "EventSkip.csproj").read_text(encoding="utf-8")


def require(text: str, label: str) -> None:
    if text not in SOURCE:
        raise AssertionError(f"missing {label}: {text}")


require("eventModel is Neow", "Neow classification")
require("return 100;", "Neow 100 gold reward")
require("return IsAncient(eventModel) ? 200 : 0;", "Ancient 200 / ordinary 0 reward")
require('eventModel.LocTable, "ancients"', "The Architect ancient-table classification")
require("PlayerCmd.GainGold(reward, owner)", "gold reward command")
require("AncientDoneMethod.Invoke", "Ancient clean completion and run history")
require("SetEventFinishedMethod.Invoke", "ordinary event clean completion")
require("options.Count == 0", "do not turn normal event completion into another skip page")
require("state.ChoiceCommitted", "single initial skip opportunity")
require("EventOptionChosenPatch", "normal choice commitment patch")
require("FakeMerchantReadyPatch", "custom Fake Merchant skip label")
require('["EVENT_SKIP.title"] = "跳过"', "Simplified Chinese localization")

if '"dependencies": []' not in MANIFEST:
    raise AssertionError("manifest must not depend on BaseLib")
if "BaseLib" in PROJECT:
    raise AssertionError("project must not reference BaseLib")

print("EventSkip offline source checks passed.")
