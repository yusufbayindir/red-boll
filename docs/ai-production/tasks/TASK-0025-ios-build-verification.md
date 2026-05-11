# TASK-0025 - iOS Workspace Build Verification

## Status

BLOCKED / Needs Config - Workspace readable, simulator build blocked before compile because no iOS Simulator destination is eligible for the `Unity-iPhone` scheme.

## Owner

iOS Workspace Build Verifier

## PM Intent

Verify the generated Unity iOS workspace from the existing `Red Ball > Build Workspace` flow without changing product code, Unity scenes/prefabs, Xcode project settings, signing configuration, or generated build outputs.

## Scope

In:

- Confirm `Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace` exists.
- Run `xcodebuild -list` on the workspace.
- Attempt an iOS Simulator Debug build where possible.
- Record first failure, likely root cause, and next recommended owner action.
- Update docs/status/debt only.

Out:

- Unity C# or scene/prefab edits.
- Xcode project, workspace, scheme, entitlement, signing, provisioning, or Apple account changes.
- Real device signing work.
- Generated build output, DerivedData, or log commits.
- Closing TD-0006 manual/mobile traversal QA.

## Acceptance Criteria

- Workspace path existence is recorded.
- `xcodebuild -list -workspace Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace` is rerun and result recorded.
- Simulator build attempt is run or explicitly blocked with command output.
- PASS or BLOCKED/Needs Config outcome is documented with first error and root cause.
- TD-0006 remains open.
- Any simulator/signing/config debt is linked in the technical debt register.
- Final report states technical debt added.

## Activity Log

- 2026-05-12 - Confirmed `Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace` exists.
- 2026-05-12 - Reran `xcodebuild -list`; PASS, schemes are `GameAssembly`, `Unity-iPhone`, and `UnityFramework`.
- 2026-05-12 - Attempted Debug iOS Simulator build for `Unity-iPhone`; BLOCKED before compile because no `generic/platform=iOS Simulator` destination matches the scheme.
- 2026-05-12 - Confirmed Xcode has `iphonesimulator26.0` and available iOS 26.0 simulator devices, while the selected target reports `SUPPORTED_PLATFORMS = iphoneos`.

## Changed Files

- `docs/ai-production/reports/ios-workspace-build-verification.md`
- `docs/ai-production/tasks/TASK-0025-ios-build-verification.md`
- `docs/ai-production/STATUS_BOARD.md`
- `docs/ai-production/technical-debt/TECHNICAL_DEBT_REGISTER.md`

## Verification

Commands run:

- `test -e 'Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace'`
- `xcodebuild -list -workspace 'Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace'`
- `xcodebuild -workspace 'Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace' -scheme Unity-iPhone -configuration Debug -sdk iphonesimulator -destination 'generic/platform=iOS Simulator' build`
- `xcodebuild -workspace 'Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace' -scheme Unity-iPhone -showdestinations`
- `xcodebuild -showsdks`
- `xcrun simctl list devices available`
- `xcodebuild -workspace 'Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace' -scheme Unity-iPhone -configuration Debug -sdk iphonesimulator -showBuildSettings`

Result:

- Workspace discoverability: PASS.
- Scheme listing: PASS.
- Simulator build: BLOCKED / Needs Config.

First error:

```text
xcodebuild: error: Unable to find a destination matching the provided destination specifier:
        { generic:1, platform:iOS Simulator }
```

Likely root cause:

`Unity-iPhone` currently exposes `SUPPORTED_PLATFORMS = iphoneos`, so Xcode does not treat iOS Simulator as an eligible destination for the generated project target.

## QA Notes

This task does not close TD-0006. Real traversal/mobile QA still needs gameplay evidence across the required manual/mobile matrix.

## Technical Debt Added

TD-0012.
