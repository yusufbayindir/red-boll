# Red Ball AI Production System

This folder is the operating system for the Red Ball project work.

The project has a PM/moderator, independent specialist roles, task files, QA gates, and repo-control rules. Every meaningful implementation task must move through:

1. PM opens or approves a task MD.
2. Developer updates the task MD before code edits.
3. Developer implements in small, modular steps.
4. Developer updates the task MD after each code step.
5. QA checks against acceptance criteria.
6. Git Repo Controller checks repo hygiene and changed files.
7. PM closes the task only after QA and repo control pass.

## Current Project Read

- Local workspace: `/Users/yusufbayindir/Desktop/ai game/red_ball`
- Remote repo requested by owner: `https://github.com/yusufbayindir/red-boll.git`
- Current local folder is not a git repository at setup time.
- Unity version from README: `6000.4.5f1`
- Main gameplay code: `Assets/Scripts/RedBallRuntime.cs`
- Main runtime file size at setup: `2338` lines.
- Current game includes 13 levels, touch controls, hearts, local progress, moving platforms, bounce pads, hazards, coins, patrol enemies, main menu, and level select.

## Non-Negotiables

- No role agrees by default. Every specialist must argue from its discipline.
- If there is a meaningful disagreement, agents debate up to 4 rounds.
- If unresolved after 4 rounds, PM asks Yusuf with concrete options.
- Code must trend modular. New systems should not casually expand the monolithic runtime file.
- Every implementation step updates the related task MD.
- QA must verify before a task can close.
- Git Repo Controller must check changed files before a task can close.

