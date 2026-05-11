# Level Design Expansion Plan

## Source

Agent: Banach  
Task: `TASK-0005`  
Date: 2026-05-11

## PM-Relevant Thesis

Do not solve expansion by adding seven longer versions of current levels. The current 13 already repeat horizontal platform chains, hazards, patrols, bounce pads, and moving platforms. Expansion needs readable structure: checkpoints, vertical motion, optional routes, and a small number of interactive objects.

## New Mechanics

| Mechanic | Recommendation | Visual Language | Mobile Fairness Rule |
| --- | --- | --- | --- |
| Vertical lifts | Use now | Existing moving half-platform tiles, vertical coin stacks | First lift over safe floor, speed <= 1.4 |
| Checkpoints | Must add | Yellow body circle + small flag, distinct from green goal | Required every 25-35 world units after Level 13 |
| Warning signs | Must add | `tile_exclamation` before danger/timing beats | Always before crumble/gate/hazard surprises |
| Crumble platforms | Add carefully | `tile_grey`, shake, delayed fall | First two over safe ground, delay >= 0.6s |
| Switch gates | Add after checkpoints | Yellow floor button opens grey tile gate | Gate and switch visible in same camera view when taught |
| Autoscroll/chase/crushers | Reject for now | Too punishing with hearts/mobile joystick | Not worth fairness risk yet |

## 20-Level Progression

| Level | Name | Purpose |
| --- | --- | --- |
| 1 | Yuvarlanma Dersi | Movement/readability |
| 2 | Ilk Tehlike | Hazard + patrol intro |
| 3 | Sekme Dersi | Bounce intro |
| 4 | Ince Adim | Jump spacing |
| 5 | Devriye Yolu | Enemy timing |
| 6 | Hareketli Kopru | Moving bridge |
| 7 | Sekme Merdiveni | Vertical bounce |
| 8 | Ritim Koprusu | Alternating rhythm |
| 9 | Yuksek Hat | Higher route |
| 10 | Inis Cikisi | Height changes |
| 11 | Devriye Kapisi | Enemy density |
| 12 | Keskin Sekme | Advanced bounce |
| 13 | Son Kosu | Current finale |
| 14 | Asansor Bahcesi | Teach vertical lifts/checkpoint |
| 15 | Kirik Taslar | Teach crumble |
| 16 | Kapi Dugmesi | Teach switch gate |
| 17 | Iki Yol | Optional risk route |
| 18 | Ritim Asansoru | Lift + crumble + patrol remix |
| 19 | Anahtar Devriyesi | Gates under pressure |
| 20 | Kirmizi Zirve | Fair final exam |

## Implementation Priority

1. Add checkpoint support before long new levels.
2. Add `AddWarningSign` as decoration-only.
3. Use existing `AddMovingPlatform` for vertical lifts and verify player ride behavior.
4. Add crumble helper/component.
5. Add switch/gate helper/component.
6. Build Levels 14-16 first and test mobile fairness.
7. Add Levels 17-20 only after QA passes readability.

## PM Note

Checkpoint support is now a prerequisite for any post-13 long-level expansion. This should become its own implementation task after architecture split planning.

