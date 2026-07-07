# PRD: [Working Title] — Physical Parkour Runner (Kinect)

**Author:** Mokshagna
**Status:** Draft v1
**Last updated:** July 2026

---

## 1. One-line pitch

A first-person, physically-controlled parkour chase game where the player's real running motion — tracked via Xbox One Kinect — determines their in-game speed, with a cop closing the distance whenever the player slows down, stumbles, or stops.

## 2. Why this project exists

- Portfolio gap: existing work (everyaesthetic, nodum, Slacker Tracker, SquareOne) is screen-based and single-user. This is physical, embodied, spatial — a different register aimed at creative-technologist/physical-interaction roles (Local Projects, museum/installation work) and directly connects to real "Who Eats Whom" exhibit experience at iEXCEL.
- Design contribution worth having: the running-cadence-drives-speed mechanic is not present in existing "gesture-control Subway Surfers" projects (which all puppet the real app via webcam + keypress simulation). This project is an original small game, not a control-remap of someone else's IP.
- Explicitly scoped as a **weekend-timeboxed side project**, not a flagship — core loop first, polish only if the loop proves fun.

## 3. Goals / non-goals

**Goals**
- A playable, fun core loop: run, dodge obstacles (vault/duck/jump), avoid getting caught by a cop, driven by real physical running-in-place.
- First-person camera, not third-person.
- Runs on Xbox One Kinect (Kinect v2 sensor) + Kinect Adapter for Windows + Unity.
- Primary build uses open-source/free asset-store assets — no custom modeling, minimal custom animation.

**Non-goals (explicitly out of scope for v1)**
- True free-form parkour (wall-running/ledge-grabbing across arbitrary geometry, procedural IK). Parkour is *sold through animation/obstacle variety* on a 3-lane track, not built as a real traversal system.
- Custom character art or original 3D asset creation.
- Public web-playable build (Kinect requirement makes this a demo/video project, not a "try it now" portfolio link).
- The "secret Subway Surfers mode" — private/local only, never shown in public builds, demos, repos, or portfolio material. Separate branch/build config, excluded from anything distributed.
- Multiplayer, leaderboards, mobile, monetization.

## 4. Target platform

- **Hardware:** Xbox One Kinect (Kinect v2) + Kinect Adapter for Windows (USB 3.0 dongle)
- **Software:** Kinect for Windows SDK 2.0 + Runtime 2.0, unofficially functional on Windows 11 (confirm on target dev machine early — treat as a Week 1 risk item)
- **Engine:** Unity (per existing familiarity), using a community Kinect v2 wrapper (e.g. "Kinect v2 with MS-SDK" on Asset Store) for skeleton joint data
- **Distribution:** Local build / demo-day / portfolio video only. Not a hosted playable.

## 5. Core mechanics

### 5.1 Locomotion — cadence-driven speed
- Track vertical oscillation (position + frequency) of knee or ankle joints over a rolling time window to compute a **running cadence value**.
- Cadence maps to forward speed: not running = speed decays toward 0; light jog = low speed; hard running-in-place = max speed.
- This is the mechanic's core differentiator — prioritize getting this feeling good over any other system.

### 5.2 Lane switching
- 3 fixed lanes (left/center/right), matching the Subway-Surfers-style lateral traffic model (as opposed to Temple Run's swipe-to-turn model).
- Player lateral hip position (relative to a calibrated center) buckets into one of 3 zones → lerp toward corresponding lane transform.

### 5.3 Obstacle types (three only, each mapped to one gesture)
| Obstacle | Gesture | Detection |
|---|---|---|
| Waist-high (dumpster, railing) | Vault | knee lift / forward lean threshold |
| Overhead (pipe, sign, fire escape) | Duck | shoulder-to-hip compression, or head joint drop below threshold |
| Ground gap | Jump | hip-center Y-position crosses threshold above rolling baseline |

Each maps to a distinct animation state so the game *reads* as parkour even though state logic is simple (lane position + one of three trigger states).

### 5.4 Cop-distance tension system (replaces binary game-over-on-hit)
- Single float, 0–100 ("cop distance," 0 = caught).
- Obstacle hit / stumble → cop closes distance by X.
- Clean run at high cadence → distance slowly regains toward max.
- Stopping/low cadence → passive distance decay even with no obstacle hits (this is the direct payoff of the original "stopping lets the cop catch up" idea).
- Game over only when distance hits 0 — not on first mistake. Escalate audio (siren volume) and visual (screen vignette/red tint) pressure as distance closes, for cheap high-impact tension.

## 6. Build order (recommended sequence — validate before adding hardware)

1. **Core loop, keyboard input, placeholder art.** Lane-transform runner (fork/adapt from Gkanatsios tutorial or Unity's official EndlessRunnerSampleGame "Trash Dash"), first-person camera on the lane transform, 3 obstacle types wired to keypresses, cop-distance system, basic HUD (distance bar + lane indicator). **Ship this before touching Kinect at all** — debugging game feel via keyboard is far faster than iterating through a skeleton feed.
2. **Validate fun.** If the core loop isn't compelling on keyboard, stop and reassess before investing in hardware integration.
3. **Kinect integration.** Swap keyboard controller for skeleton-driven input: cadence → speed, hip lateral position → lane, gesture thresholds → vault/duck/jump. Reuse same input interface/abstraction so this is a drop-in swap, not a rewrite.
4. **Asset pass.** Replace placeholders with open-source/asset-store assets (character, environment, cop character, SFX).
5. **(Optional, time-permitting) Parkour animation polish** — vault/duck/jump animations, lane-switch reading as wall-kick/dive-roll rather than a flat slide, camera head-bob driven by the real cadence signal rather than a placeholder sine wave.
6. **(Private, local-only, optional) Secret asset-swap mode** — separate build config, never merged into anything public.

## 7. Assets (open-source / free-first approach)

- Environment: Unity Asset Store free "urban street/alley" packs, Sketchfab CC-licensed props, Kenney.nl (free, CC0, includes obstacle/prop packs well-suited to placeholder-to-final use)
- Character: Mixamo (free rigged characters + animations — vault/jump/duck/run cycles likely available directly, minimizing custom animation work)
- Cop character/AI: reuse Mixamo rig; simple chase-adjacent visual (doesn't need real pathing AI since "cop distance" is an abstracted float, not a simulated pursuer navigating the level)
- Audio: freesound.org (siren, footsteps, ambient city) — CC0/attribution tracks only

## 8. Risks

| Risk | Mitigation |
|---|---|
| Kinect v2 SDK reliability on Windows 11 | Confirm sensor + adapter + SDK working end-to-end in Week 1, before any game logic work |
| Scope creep (matches a documented pattern of exploring many directions vs. finishing one) | Hard timebox on core loop (1–2 weekends); parkour polish and secret mode explicitly deferred/optional |
| Cadence detection doesn't feel good / is unreliable | Validate detection logic in isolation (simple debug scene showing live cadence value) before wiring to full game |
| First-person lane awareness is hard to read | Ground lane markers (rail lines/glow), possible minimal HUD lane indicator |
| Motion sickness | Likely mitigated by physical running-in-place providing real vestibular feedback matching visual motion — worth noting as a design point, not just a risk |

## 9. Definition of done (v1)

- Playable end-to-end on Kinect hardware: player physically runs in place to move, switches lanes via body position, vaults/ducks/jumps three obstacle types, cop-distance system creates real tension and can end the run.
- At least one clean recorded demo video (for portfolio/reel use).
- Short devlog/write-up specifically explaining the cadence-drives-speed design decision — this is the differentiator and needs to be documented, not just built, or it won't read as original when reviewed.

## 10. Open questions

- Exact cadence → speed curve (linear vs. eased) — needs playtesting, not a spec decision.
- Whether cop is visually rendered (chasing figure) or purely abstracted as a UI/audio distance value — visual cop is more legible but adds animation/AI overhead for zero mechanical benefit if distance is already a float.
- Adapter compatibility confirmation (does the specific Xbox One Kinect already have the right adapter, or does one need to be purchased) — verify before Week 1 starts.
