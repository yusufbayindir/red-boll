# TASK-0027 - iOS Simulator Build Strategy

## Status

Completed - PM decision recommended / TD-0012 criteria updated.

## Owner

iOS Simulator Build Strategy Engineer

## PM Intent

Investigate why the Unity-generated iOS workspace cannot build for iOS Simulator, identify the settings/files involved, and propose a Sprint 01 decision path. Do not write product code or change Unity/Xcode settings.

## Scope

In:

- Research Unity iOS Target SDK requirements and Xcode platform settings using primary sources where web research is used.
- Read relevant local settings/export files: `ProjectSettings`, generated iOS Xcode project, schemes, and config files if present.
- Produce 2-3 solution paths with risk, effort, owner, and acceptance criteria.
- Recommend Sprint 01 PM decision.
- Update TD-0012 closure criteria.

Out:

- Unity Editor setting changes.
- Xcode project/scheme/signing changes.
- Re-exporting iOS builds.
- Running build commands that create new repo artifacts.
- Product code changes.

## Findings

- Unity 6000.4.5f1 is recorded in `ProjectSettings/ProjectVersion.txt`.
- Unity iOS settings store `iPhoneSdkVersion: 988`, `iOSSimulatorArchitecture: 0`, and minimum iOS `15.0`.
- The generated Xcode project repeatedly sets `SDKROOT = iphoneos` and `SUPPORTED_PLATFORMS = iphoneos`.
- No generated `.xcconfig` files were found under `Builds/iOS/RedBall-iOS`.
- The `Unity-iPhone` scheme points at the `Unity-iPhone` app target; it does not override platform support.
- Official Unity docs confirm simulator builds require selecting Simulator SDK in iOS Target SDK; Device SDK plus Xcode Simulator target is expected to fail.

## Changed Files

- `docs/ai-production/reports/ios-simulator-build-strategy.md`
- `docs/ai-production/tasks/TASK-0027-ios-simulator-build-strategy.md`
- `docs/ai-production/STATUS_BOARD.md`
- `docs/ai-production/technical-debt/TECHNICAL_DEBT_REGISTER.md`

## Verification

Read-only commands used:

- `rg --files`
- `git status --short`
- `rg -n "TD-0012|ios simulator|iOS Simulator|SUPPORTED_PLATFORMS|SDK|PlayerSettings|TASK-0027|technical debt" ...`
- `find ProjectSettings Builds docs ...`
- `sed` reads of TASK-0025, iOS verification report, status board, TD register, ProjectSettings, ProjectVersion, pbxproj, and xcscheme.
- `rg -n "SDKROOT|SUPPORTED_PLATFORMS|IPHONEOS_DEPLOYMENT_TARGET|ARCHS|UNITY_RUNTIME_VERSION|iPhoneSdkVersion|iOSSimulatorArchitecture" ...`

No code, Unity settings, generated Xcode settings, signing configuration, or build outputs were changed.

## Recommendation

For Sprint 01, choose device-only Xcode workspace plus physical-device QA if signing/device access exists, backed by Editor/CI PlayMode/EditMode evidence where native iOS execution is unavailable. Defer simulator SDK re-export to Sprint 02 under TD-0012.

Do not close simulator build verification from the current workspace. Do not hand-edit `project.pbxproj` as the primary fix; the cleaner remediation is a Unity simulator SDK export.

## Acceptance Criteria

- Strategy report exists with source-backed findings.
- Local config/export evidence is documented.
- Options A/B/C are documented with risk, effort, owner, and acceptance criteria.
- PM recommendation is explicit.
- TD-0012 closure criteria are updated.
- Final technical debt line is present.

## Technical Debt Added

Technical debt added: none
