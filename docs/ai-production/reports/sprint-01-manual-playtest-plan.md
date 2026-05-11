# Sprint 01 Manual Playtest And Mobile QA Plan

Date: 2026-05-12  
Owner: Manual Playtest / Mobile QA Planner  
Source task: `TASK-0024-sprint-01-manual-playtest-mobile-qa.md`  
Status: Ready to run. Not executed. Not PASS.

## Purpose

This document turns Sprint 01 QA into a manual, player-like script for Levels 14 and 15, mastery badges, persistence, Clean Run, All Coins, optional skill route, crumbling tiles, checkpoints, and mobile/iOS build verification.

This is a planning and local static-check document only. It does not change Unity code, Xcode project files, scenes, prefabs, or assets.

## Pre-Test Setup

Record before every run:

| Field | Value |
| --- | --- |
| Tester | |
| Date/time | |
| Build source | |
| Unity version | |
| Xcode version | |
| iOS device/simulator | |
| Orientation | Landscape |
| Save state used | Fresh / progressed / mixed badges |
| Evidence folder | |
| Console/log path | |

Before playing:

1. Confirm Sprint 01 compile gate is already PASS and EditMode smoke is already PASS.
2. Do not modify code, scenes, prefabs, project settings, Xcode settings, or signing configuration during QA.
3. Start from a known save state: fresh save, progressed save, or mixed badge save. Record which one is used.
4. Capture screenshots or short clips for level select badge display, completion feedback, Level 14 lift, Level 15 crumbling tile, checkpoint respawn, and mobile readability.
5. Keep Console visible in Unity Editor runs, or keep Xcode console/device logs visible for iOS runs.
6. After every failed scenario, record whether the failure is a bug candidate, blocked setup, or PM-accepted risk candidate. Do not mark Sprint 01 PASS from this plan.

## Result Codes

Use one result per scenario:

- Pass: scenario was executed and matched expected result.
- Fail: scenario was executed and violated expected result.
- Blocked: scenario could not be executed because setup/build/access was unavailable.
- Pass with risk: scenario matched the minimum behavior but has a PM-accepted limitation already recorded as debt.
- Not run: scenario is still pending.

## Manual QA Script

| ID | Area | Setup | Steps | Expected result | Result | Evidence / notes |
| --- | --- | --- | --- | --- | --- | --- |
| MQA-01 | Fresh save baseline | Clear local prototype save or use a fresh profile. | 1. Launch app. 2. Open main menu. 3. Open level select. 4. Enter Level 1. 5. Return to level select. | App reaches menu, level select, and Level 1 with no console errors. Level 1 is unlocked. No badges are incorrectly pre-earned. | Not run | |
| MQA-02 | Progressed save compatibility | Use a save with older unlock/completion data and no Sprint 01 badge data. | 1. Launch app. 2. Open level select. 3. Enter a previously unlocked level. 4. Return to level select. | Existing unlock/completion data remains intact. New badge fields default safely and do not relock levels. | Not run | |
| MQA-03 | Clear only badge | Choose a coin-bearing level with a reachable goal. | 1. Start level. 2. Skip at least one coin. 3. Damage/fall is allowed. 4. Reach goal. 5. Observe completion feedback. | Clear is awarded. All Coins is not awarded. Clean Run is awarded only if no damage/fall/death happened. | Not run | |
| MQA-04 | All Coins with damage | Choose a coin-bearing level with a safe damage/fall opportunity. | 1. Collect every coin. 2. Take damage or trigger fall/death during the same attempt. 3. Finish level. | Clear and All Coins are awarded. Clean Run is not awarded. | Not run | |
| MQA-05 | Clean Run without all coins | Choose a level with optional coins. | 1. Start a fresh attempt. 2. Skip at least one coin. 3. Avoid all damage, enemy contact, hazard damage, fall, and death. 4. Finish level. | Clear and Clean Run are awarded. All Coins is not awarded. | Not run | |
| MQA-06 | Full mastery | Choose a coin-bearing level. | 1. Start a fresh attempt. 2. Collect all coins. 3. Avoid all damage/fall/death. 4. Finish level. | Clear, All Coins, and Clean Run are all awarded in completion feedback and level select. | Not run | |
| MQA-07 | Badge persistence after menu | Earn a known badge set. | 1. Return to main menu. 2. Reopen level select. 3. Re-enter the same level. 4. Return again to level select. | Earned badge state persists through menu navigation and does not duplicate, disappear, or mislabel badges. | Not run | |
| MQA-08 | Badge persistence after relaunch | Earn a known badge set and record it. | 1. Quit app/stop Play mode. 2. Relaunch app. 3. Open level select. | Earned badge state persists after relaunch. Legacy unlock/completion data remains valid. | Not run | |
| MQA-09 | Replay badge improvement | Start from a level with Clear only. | 1. Replay and earn All Coins. 2. Return to level select. 3. Replay again and earn Clean Run if still missing. | Previously earned badges remain. Missing badges can be added on later runs. Progression is not relocked. | Not run | |
| MQA-10 | Clean Run invalidation matrix | Use one or more levels with enemy/hazard/fall paths. | 1. In attempt A, take enemy side damage and finish. 2. In attempt B, take hazard damage and finish. 3. In attempt C, fall/death and finish after respawn. | Any damage, enemy damage, hazard damage, fall, or death invalidates Clean Run for that attempt. | Not run | |
| MQA-11 | Restart resets badge attempt | Use any badge-capable level. | 1. Start attempt. 2. Take damage or fall. 3. Use restart. 4. Complete with all coins and no damage/fall after restart. | Restart begins a fresh badge attempt. Prior failed clean-run state does not leak into the restarted attempt. | Not run | |
| MQA-12 | Level select badge readability | Prepare mixed badge states across multiple levels. | 1. Open level select on desktop/editor. 2. Open on phone-landscape iOS build or simulator. 3. Inspect locked/current/completed states and badge indicators. | Badge indicators are compact, readable, and do not obscure level selection or tap targets. | Not run | |
| MQA-13 | Level 14 reachability | Progress/unlock Level 14 or use approved QA access. | 1. Open level select. 2. Enter Level 14. 3. Wait 5 seconds before moving. | Level 14 loads with player, HUD, controls, route, hazards, coins, and goal visible enough to play. No startup console errors. | Not run | |
| MQA-14 | Level 14 vertical lift teaching | Start Level 14 fresh. | 1. Approach first vertical lift slowly. 2. Ride it once without jumping. 3. Ride again and jump off. 4. Intentionally miss once if safe. | First lift is readable and recoverable. It does not force surprise death. Player can learn timing before high pressure. | Not run | |
| MQA-15 | Level 14 safe route Clear | Start Level 14 fresh. | 1. Follow the main safe route. 2. Ignore optional/skill coins. 3. Reach goal. | Level 14 can be cleared without taking optional skill route. Clear is awarded. | Not run | |
| MQA-16 | Level 14 skill route / All Coins | Start Level 14 after safe route is understood. | 1. Take optional lift/coin route. 2. Collect all available coins. 3. Recover from a missed optional jump if the route permits. 4. Finish level. | Optional route is reachable and readable. All Coins is possible without making Clear impossible after a reasonable miss. | Not run | |
| MQA-17 | Level 14 checkpoint respawn | Start Level 14 and reach checkpoint. | 1. Activate checkpoint. 2. Take damage or fall after checkpoint. 3. Observe respawn. 4. Finish or retry. | Player respawns at latest checkpoint according to existing heart rules. No soft-lock. Clean Run is invalidated for that attempt. | Not run | |
| MQA-18 | Level 15 reachability | Progress/unlock Level 15 or use approved QA access. | 1. Open level select. 2. Enter Level 15. 3. Wait 5 seconds before moving. | Level 15 loads with player, HUD, controls, route, crumbling tiles, coins, and goal visible enough to play. No startup console errors. | Not run | |
| MQA-19 | Level 15 crumbling tile teaching | Start Level 15 fresh. | 1. Step onto first crumbling tile. 2. Pause/hesitate long enough to observe warning timing. 3. Retreat or recover if possible. 4. Repeat with a normal crossing. | First crumbling tile teaches behavior safely. Delay, visual change, and disappearance are readable before pit pressure. | Not run | |
| MQA-20 | Level 15 crumble-over-pit | Reach later pit/timing section. | 1. Cross crumbling tiles at normal pace. 2. Intentionally fail once. 3. Respawn and retry. 4. Finish section. | Pit timing is fair after teaching. Failure respawns cleanly and does not leave an impossible world state. | Not run | |
| MQA-21 | Crumbling tile reset by restart/reload | In Level 15, collapse one or more crumbling tiles. | 1. Use manual restart. 2. Confirm tiles restored. 3. Collapse tiles again. 4. Exit/reload Level 15. 5. Confirm tiles restored. | Manual restart and full level reload restore crumbling tiles. If they do not, this is a blocker candidate. | Not run | |
| MQA-22 | Crumbling tile and checkpoint interaction | In Level 15, use any checkpoint near crumble content if present. | 1. Activate checkpoint. 2. Collapse crumbling tile(s). 3. Take damage/fall. 4. Observe checkpoint respawn. 5. Attempt to continue. | No soft-lock after checkpoint respawn. Known TD-0009 risk is acceptable only if route remains completable and PM accepts it. | Not run | |
| MQA-23 | Checkpoint and Clean Run interaction | Use Level 14 or 15 checkpoint flow. | 1. Activate checkpoint. 2. Take damage/fall. 3. Finish level. 4. Restart from a fresh attempt and finish cleanly. | Damage/fall after checkpoint invalidates Clean Run for that attempt. A new attempt can earn Clean Run. | Not run | |
| MQA-24 | Mobile controls traversal | Use iPhone landscape build or simulator. | 1. Traverse Level 14 safe route. 2. Traverse Level 15 first crumbling section. 3. Use joystick/jump without keyboard. | Touch controls remain usable. HUD and controls do not hide hazards, lift timing, crumble timing, coins, or goal. | Not run | |
| MQA-25 | No retry monetization regression | Use low hearts and normal hearts if available. | 1. Fail/retry Level 14. 2. Fail/retry Level 15. 3. Replay for badges. 4. Return to menu and re-enter. | No forced ad, wait timer, energy cost, currency cost, or new retry gate appears in mastery/retry flow. Existing heart behavior remains as designed. | Not run | |

## iOS Build And Xcode Test Path

Use this path for mobile/iOS QA. Do not edit Xcode project settings during the QA pass.

1. Open the Unity project at `/Users/yusufbayindir/Desktop/ai game/red_ball`.
2. In Unity, use `RedBall > Build Workspace` to generate and open the Xcode workspace. The editor script targets `Builds/iOS/RedBall-iOS`.
3. If the workspace already exists and no rebuild is needed, use `RedBall > Open Build Panel > Open Build Workspace`, or open:
   `/Users/yusufbayindir/Desktop/ai game/red_ball/Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace`
4. In Xcode, select the `Unity-iPhone` scheme.
5. Use a landscape-capable iPhone device or simulator. Preferred minimum matrix:
   - Small/notched phone: iPhone SE (3rd generation) or smallest available iPhone simulator/device.
   - Modern standard phone: iPhone 15/16/17 class simulator/device.
   - Large phone: iPhone 15/16/17 Pro Max class simulator/device.
   - Optional tablet viewport: iPad 10th generation or similar.
6. Before pressing Run, check:
   - Workspace opens, not only the `.xcodeproj`.
   - Scheme is `Unity-iPhone`.
   - Target device is an iPhone/iPad simulator or a connected iOS device, not "Any iOS Device" for manual play.
   - Signing is not manually edited during QA. If signing blocks physical device launch, record Blocked and use simulator for viewport/readability coverage.
   - Orientation is landscape in the running app.
   - Console/device logs are visible.
7. Run the build from Xcode if needed. If build fails, record the error summary and mark iOS manual QA Blocked for build availability. Do not change code or Xcode settings to fix it inside this QA task.
8. Execute mobile rows MQA-12, MQA-24, and the Level 14/15 traversal rows on the selected devices/viewports.

## Local Static Checks Performed

These checks were performed from `/Users/yusufbayindir/Desktop/ai game/red_ball` on 2026-05-12. They are not gameplay QA and do not create a PASS verdict.

| Check | Result |
| --- | --- |
| Workspace directory exists | Found `/Users/yusufbayindir/Desktop/ai game/red_ball/Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace`. |
| Xcode project exists | Found `/Users/yusufbayindir/Desktop/ai game/red_ball/Builds/iOS/RedBall-iOS/Unity-iPhone.xcodeproj`. |
| Workspace contents file exists | Found `Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace/contents.xcworkspacedata`. |
| Project file exists | Found `Builds/iOS/RedBall-iOS/Unity-iPhone.xcodeproj/project.pbxproj`. |
| `xcodebuild -list` workspace readability | Succeeded. Schemes listed: `GameAssembly`, `Unity-iPhone`, `UnityFramework`. |
| Available simulator device types | `xcrun simctl list devicetypes` returned multiple iPhone and iPad device types including modern iPhone and iPad profiles. |
| Build execution | Not run. No code, Unity project, or Xcode project changes were made for this plan. |

## TD-0006 Closure Criteria

TD-0006 can move out of Open only after evidence exists for all items below, or PM explicitly accepts the remaining risk in writing.

Required device/viewport coverage:

- At least 3 phone-landscape viewports/devices: one small phone, one standard modern phone, and one large/notched phone.
- At least 1 desktop/editor or simulator baseline run for fast logging.
- Optional but recommended: 1 iPad/tablet landscape viewport before public launch capture.

Required traversal coverage:

- Level 14 safe route Clear from start to goal.
- Level 14 optional skill route with All Coins attempt.
- Level 14 checkpoint respawn after damage/fall.
- Level 15 safe route Clear from start to goal.
- Level 15 first crumbling tile teaching interaction.
- Level 15 later crumbling-over-pit interaction.
- Level 15 crumbling tile reset by restart and full level reload.
- Level 15 crumbling tile plus checkpoint/fall interaction, with no soft-lock.
- Representative regression traversal for Levels 1, 3, 5, and 13.

Required badge cases:

- Fresh save has no false earned badges.
- Existing save compatibility preserves prior unlock/completion state.
- Clear only.
- All Coins with damage/fall, proving Clean Run is not awarded.
- Clean Run without all coins, proving All Coins is not awarded.
- Full mastery: Clear + All Coins + Clean Run.
- Clean Run invalidation for enemy damage, hazard damage, fall/death, and checkpoint respawn after failure.
- Replay improvement adds missing badges without losing prior badges.
- Persistence after menu return and app relaunch.

Required evidence:

- Completed result table with Pass/Fail/Blocked/Pass with risk for every MQA row.
- Screenshots or short clips for badge display, completion feedback, Level 14 lift, Level 15 crumble, checkpoint respawn, and mobile HUD/touch control readability.
- Console/device log summary with any errors copied exactly.
- Device/viewport list, build source, Unity version, and Xcode version.
- Any accepted limitation linked to a technical-debt register entry before closure.

## Technical Debt

Technical debt added: none.

Existing debt referenced: `TD-0006`, `TD-0009`.
