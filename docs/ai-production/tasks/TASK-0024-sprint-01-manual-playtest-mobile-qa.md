# TASK-0024 - Sprint 01 Manual Playtest / Mobile QA

## Status

Ready to run - Manual/mobile QA plan prepared, execution pending

## Owner

Manual Playtest / Mobile QA Planner

## PM Intent

Create a ready-to-run manual QA script and iOS/mobile execution path for Sprint 01 without PM writing code and without changing Unity or Xcode project files.

## Scope

In:

- Manual QA scenarios for Level 14, Level 15, badge persistence, Clean Run, All Coins, skill route, crumbling tiles, and checkpoint interactions.
- iOS/Xcode workspace test path using the existing `RedBall > Build Workspace` flow.
- Local shell validation of Xcode workspace/project discoverability where possible.
- Clear TD-0006 closure criteria.
- Status board and technical debt documentation updates.

Out:

- Product/runtime code changes.
- Unity scene, prefab, asset, package, or project setting changes.
- Xcode project, signing, scheme, entitlement, or workspace edits.
- Claiming Sprint 01 QA PASS before manual play evidence exists.

## Acceptance Criteria

- `docs/ai-production/reports/sprint-01-manual-playtest-plan.md` exists.
- Manual QA script includes step-by-step player actions, expected result, and Pass/Fail recording fields.
- iOS test path documents Unity `RedBall > Build Workspace`, Xcode workspace path, scheme/device guidance, and pre-run checks.
- Local static workspace/build-openability checks are recorded if available.
- TD-0006 closure criteria define required devices/viewports, traversal coverage, badge cases, and evidence.
- QA result remains Ready to run / Pending, not PASS.
- Final technical debt line is present.

## Activity Log

- 2026-05-12 - Created ready-to-run manual playtest/mobile QA plan for Sprint 01. Verified existing Xcode workspace/project paths and `xcodebuild -list` scheme readability without running a build or changing code.

## Changed Files

- `docs/ai-production/reports/sprint-01-manual-playtest-plan.md`
- `docs/ai-production/tasks/TASK-0024-sprint-01-manual-playtest-mobile-qa.md`
- `docs/ai-production/STATUS_BOARD.md`
- `docs/ai-production/technical-debt/TECHNICAL_DEBT_REGISTER.md`

## Verification

Local static checks only:

- `find . -maxdepth 5` found `Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace`.
- `find . -maxdepth 5` found `Builds/iOS/RedBall-iOS/Unity-iPhone.xcodeproj`.
- `find Builds/iOS/RedBall-iOS -maxdepth 3` found workspace contents, Xcode project file, and iOS `Info.plist` files.
- `xcodebuild -list -workspace Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace` exited 0 and listed schemes: `GameAssembly`, `Unity-iPhone`, `UnityFramework`.
- `xcrun simctl list devicetypes` returned available iPhone/iPad simulator device types.

No Unity build, Xcode build, simulator launch, device install, or manual gameplay traversal was executed by this planning task.

## QA Notes

Sprint 01 remains QA PARTIAL / manual play pending. This task prepares execution and local static build-openability checks only.

Manual execution source of truth: `docs/ai-production/reports/sprint-01-manual-playtest-plan.md`.

## Technical Debt Added

none
