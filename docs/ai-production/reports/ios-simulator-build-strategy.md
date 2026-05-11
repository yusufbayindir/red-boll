# iOS Simulator Build Strategy

Date: 2026-05-12

Owner: iOS Simulator Build Strategy Engineer

Scope: Docs-only investigation and decision recommendation for TD-0012. No Unity, Xcode, generated build, signing, scene, prefab, script, or project setting files were changed.

## Executive Recommendation

Keep the current device-only Unity Xcode workspace open for Sprint 01, and move simulator build remediation to Sprint 02 under TD-0012.

Reason: Sprint 01 is already blocked on real play/mobile evidence under TD-0006. Forcing a simulator export/settings change now would touch the Unity iOS build surface and regenerate Xcode output during QA stabilization. That has a higher churn risk than the value it adds this sprint. The PM should accept that Sprint 01 can continue with editor/EditMode evidence plus a physical-device QA path if signing is available, while simulator build pass remains explicitly unclaimed.

## Primary Source Findings

- Unity iOS Player Settings expose Target SDK as Device SDK or Simulator SDK. Unity states that the selected SDK must match the Xcode target; selecting Device SDK in Unity and targeting Simulator in Xcode causes build failure. Source: https://docs.unity.cn/Manual/class-PlayerSettingsiOS.html
- Unity 6.0 system requirements list iOS/iPadOS player development on Xcode 15+ and iOS/iPadOS 13+ runtime requirements. Source: https://docs.unity3d.com/6000.0/Documentation/Manual/system-requirements.html
- Apple Xcode build settings define `SDKROOT` as the base SDK and `SUPPORTED_PLATFORMS` as the supported platforms from which a base SDK can be used. Source: https://developer.apple.com/documentation/xcode/build-settings-reference
- Apple documents `generic/platform=iOS Simulator` as an iOS Simulator destination value in Xcode build workflows. Source: https://developer.apple.com/documentation/xcode/creating-a-multi-platform-binary-framework-bundle

## Local Evidence

Read-only inspection found:

- Unity version: `ProjectSettings/ProjectVersion.txt` is `6000.4.5f1 (cc83ebd631f8)`.
- Unity project setting: `ProjectSettings/ProjectSettings.asset` has `iPhoneSdkVersion: 988`, `iOSSimulatorArchitecture: 0`, and `iOSTargetOSVersionString: 15.0`.
- Generated Xcode app target: `Builds/iOS/RedBall-iOS/Unity-iPhone.xcodeproj/project.pbxproj` sets `SDKROOT = iphoneos` and `SUPPORTED_PLATFORMS = iphoneos` for `Unity-iPhone` Debug and Release.
- Generated Xcode framework/test/support targets also contain repeated `SDKROOT = iphoneos` and `SUPPORTED_PLATFORMS = iphoneos` settings.
- Generated scheme: `Unity-iPhone.xcscheme` builds the `Unity-iPhone` app target, but does not itself add simulator platform eligibility.
- No `.xcconfig` files were found under `Builds/iOS/RedBall-iOS`.
- TASK-0025 already proved local Xcode has `iphonesimulator26.0` and available iOS 26 simulator devices; the blocker is not missing simulator runtime or signing.

Interpretation: the current export is a device SDK export. Xcode is behaving consistently: the scheme cannot resolve `generic/platform=iOS Simulator` because the selected target only supports `iphoneos`.

## Strategy Options

| Option | Path | Effort | Risk | Owner | Acceptance criteria |
| --- | --- | --- | --- | --- | --- |
| A | Re-export from Unity with iOS Target SDK set to Simulator SDK, then rerun simulator build. | Medium | Regenerates Xcode output; may disturb current device workspace; simulator artifacts are not App Store/device proof; native plugins or IL2CPP artifacts may reveal simulator-specific issues. | iOS Build / Unity Export Owner | A fresh simulator export exists in a clearly named path or documented replacement path; `xcodebuild -workspace Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace -scheme Unity-iPhone -configuration Debug -sdk iphonesimulator -destination 'generic/platform=iOS Simulator' build` reaches compile and passes; TD-0006 records simulator baseline evidence if gameplay is run. |
| B | Keep device-only build and perform physical device QA with signing/provisioning. | Low to Medium | Requires Apple signing and available device; slower iteration than simulator; does not close simulator-specific build debt. | iOS Build / QA + Yusuf for device/signing access | Xcode device destination is eligible; device Debug/ReleaseForRunning build installs and launches on a physical iPhone/iPad; manual/mobile QA evidence is recorded; PM explicitly states simulator build remains deferred under TD-0012. |
| C | Cover Sprint 01 mobile risk through Editor/CI PlayMode/EditMode plus targeted manual viewport QA, without iOS simulator/device build as a sprint gate. | Low | Does not prove native iOS packaging, touch/input, safe area, performance, signing, or install flow; should not be called mobile build verification. | QA Lead + Unity Test Harness Developer | Compile/import and EditMode/PlayMode smoke pass; manual viewport script is executed where possible; Sprint 01 release notes say iOS build verification is not passed and remains under TD-0012/TD-0006. |

## Decision Recommendation For Sprint 01

Choose B plus C for Sprint 01, and defer A to Sprint 02.

The PM's current instinct is right for the sprint shape: keep the device-only Xcode workspace readable and usable, avoid a late Unity export/config churn, and do not spend Sprint 01 on simulator plumbing unless PM/Yusuf require simulator evidence as a release gate.

Counterargument: If no physical device/signing path is available, Option A becomes more attractive because simulator is the only practical native iOS execution path. In that case, the clean version of A is not to hand-edit `project.pbxproj`; it is to produce a new Unity iOS export from Simulator SDK and treat it as a separate QA artifact.

## Proposed Sprint 02 Remediation

1. In Unity 6000.4.5f1, set iOS Player Settings > Other Settings > Target SDK to Simulator SDK for a simulator QA export.
2. Export to a separate path, for example `Builds/iOS/RedBall-iOS-Simulator`, or intentionally replace `Builds/iOS/RedBall-iOS` with a documented before/after.
3. Confirm generated Xcode settings include `iphonesimulator` eligibility for the relevant app/framework targets.
4. Run:

```sh
xcodebuild -workspace 'Builds/iOS/RedBall-iOS/Unity-iPhone.xcworkspace' -scheme Unity-iPhone -configuration Debug -sdk iphonesimulator -destination 'generic/platform=iOS Simulator' build
```

5. If build passes, run the relevant TD-0006 mobile baseline scenario on an installed simulator app.
6. If the team chooses device-only permanently, record a PM/Yusuf decision that physical-device QA replaces simulator baseline for this project.

## Unresolved User Decisions

- Is a physical iPhone/iPad available for Sprint 01 QA, with a usable Apple team/provisioning path?
- Should simulator support be required before release candidate, or only before the next iOS QA cycle?
- Should future exports maintain separate device and simulator output folders, or should the workspace be regenerated for the current QA target each time?

## Technical Debt

No new debt ID is required. TD-0012 remains the correct tracking item; its closure criteria should distinguish simulator remediation from a PM-accepted device-only QA path.

Technical debt added: none
