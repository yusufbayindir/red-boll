# iOS Workspace Build Verification

Date: 2026-05-12

Owner: iOS Workspace Build Verifier

## Result

BLOCKED / Needs Config

The generated iOS workspace exists and is readable by Xcode, but the requested iOS Simulator build did not start because the `Unity-iPhone` scheme has no eligible iOS Simulator destination.

This is not a signing failure. The first blocker is destination/platform support:

- `xcodebuild -showdestinations` lists physical iOS devices, `Any iOS Device`, and `My Mac`, but no iOS Simulator destination for `Unity-iPhone`.
- `xcodebuild -showBuildSettings -sdk iphonesimulator` reports `SUPPORTED_PLATFORMS = iphoneos` for the selected build target.
- The local Xcode install has `iphonesimulator26.0`, and `xcrun simctl list devices available` shows iOS 26.0 simulator devices, so the missing destination appears to be project/scheme platform configuration in the generated Unity Xcode workspace.

TD-0006 remains open. This verification only checks workspace readability and Xcode build reachability; it does not provide mobile traversal QA evidence.

## Workspace

Expected path:

`Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace`

Status: PASS - path exists.

## Commands Run

```sh
test -e 'Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace'
```

Result: PASS.

```sh
xcodebuild -list -workspace 'Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace'
```

Result: PASS. Schemes listed:

- `GameAssembly`
- `Unity-iPhone`
- `UnityFramework`

```sh
xcodebuild -workspace 'Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace' -scheme Unity-iPhone -configuration Debug -sdk iphonesimulator -destination 'generic/platform=iOS Simulator' build
```

Result: BLOCKED before compile.

First error:

```text
xcodebuild: error: Unable to find a destination matching the provided destination specifier:
        { generic:1, platform:iOS Simulator }
```

Available destinations reported for `Unity-iPhone`:

- `My Mac` - Designed for `[iPad,iPhone]`
- physical iOS devices
- `Any iOS Device`

No iOS Simulator destination was eligible.

```sh
xcodebuild -workspace 'Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace' -scheme Unity-iPhone -showdestinations
```

Result: PASS command execution, confirms no iOS Simulator destination listed for the scheme.

```sh
xcodebuild -showsdks
```

Result: PASS. Relevant SDKs include:

- `iOS 26.0` / `iphoneos26.0`
- `Simulator - iOS 26.0` / `iphonesimulator26.0`

```sh
xcrun simctl list devices available
```

Result: PASS. iOS 26.0 simulator devices are installed, including iPhone 17 Pro, iPhone 17 Pro Max, iPhone Air, iPhone 17, iPhone 16e, and current iPad simulator profiles.

```sh
xcodebuild -workspace 'Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace' -scheme Unity-iPhone -configuration Debug -sdk iphonesimulator -showBuildSettings
```

Relevant finding:

```text
SUPPORTED_PLATFORMS = iphoneos
SDKROOT = /Applications/Xcode.app/Contents/Developer/Platforms/iPhoneOS.platform/Developer/SDKs/iPhoneOS26.0.sdk
CODE_SIGN_STYLE = Automatic
DEVELOPMENT_TEAM = P8C3928J3Z
PRODUCT_BUNDLE_IDENTIFIER = com.yusufbayindir.redball
```

## Root Cause

The generated Unity Xcode project currently presents the app target as `iphoneos` only. Because `iphonesimulator` is not included in the eligible supported platforms for the selected scheme/target, Xcode cannot resolve `generic/platform=iOS Simulator`, even though simulator SDKs and devices exist on the machine.

## Recommended Fix

Regenerate or adjust the Unity iOS Xcode export so simulator builds are supported when simulator QA is required. The next owner should verify Unity iOS Player/Xcode export settings and Xcode target platform support, then rerun:

```sh
xcodebuild -workspace 'Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace' -scheme Unity-iPhone -configuration Debug -sdk iphonesimulator -destination 'generic/platform=iOS Simulator' build
```

If the intent is device-only iOS QA, use a physical device flow with proper signing/provisioning, but do not treat that as a simulator build pass.

## Generated Files

No repository build output, DerivedData, logs, or generated Xcode artifacts were intentionally created, staged, or committed by this verification. Xcode wrote temporary result bundles under `/var/folders/.../T/`, outside the repo.

## Technical Debt

Technical debt added: TD-0012.
