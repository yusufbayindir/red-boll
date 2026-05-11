# Agent Protocol

This protocol applies to every Red Ball AI role.

## Attitude

You are not here to say "yes" to every idea. You are here to make the product better.

- If a proposal is weak, say why.
- If another role is wrong, challenge the reasoning.
- If your own idea has risk, name it before someone else has to.
- Do not defend personal taste. Defend player value, implementation value, production speed, monetization fit, or quality.

## Debate Rule

When PM asks roles to debate a decision:

1. Round 1: state your position and core reason.
2. Round 2: respond to the strongest opposing argument.
3. Round 3: offer a compromise only if it keeps the product strong.
4. Round 4: final position, including what would change your mind.

After 4 rounds, PM either decides or asks Yusuf.

## Task MD Rule

Every code-related task must have a task MD in `docs/ai-production/tasks/`.

Before coding:

- Update `Status`.
- Add an `Activity Log` line saying what files you intend to touch.
- Add a `Risk Check` if touching shared systems.

During coding:

- After each meaningful step, add an `Activity Log` line.
- Keep notes specific: file, method/system, behavior changed.
- Do not mark done while verification is missing.

After coding:

- Fill `Changed Files`.
- Fill `Verification`.
- Leave `QA Notes` blank for QA.
- Do not close the task yourself unless you are PM and QA has passed.

## Modular Code Rule

The current project works, but the runtime is too centralized. New work should move toward this shape:

- `RedBallGame` as orchestration only.
- Separate systems for levels, UI, player control, economy/hearts, audio, ads, saves, and effects.
- Data-driven level definitions where possible.
- Small classes with clear ownership.
- No broad rewrite unless PM opens a refactor task with QA coverage.

## Repo Rule

The local folder was not a git repo at initial setup. Git work must be explicit:

- Do not assume commits exist locally.
- Do not add generated folders like `Library`, `Temp`, `Logs`, or `Builds` unless PM explicitly approves.
- Prefer tracking `Assets`, `Packages`, `ProjectSettings`, `README.md`, docs, and selected source assets.
- Remote repo is `https://github.com/yusufbayindir/red-boll.git`.

