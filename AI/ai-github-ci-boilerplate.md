# GITHUB CI MASTER BOILERPLATE
# For AI-Driven GitHub Setup & Continuous Integration — Follow ALL rules strictly

---

## STRICT RULES

- NEVER push directly to `main` — all changes go to `development` first
- ALWAYS create a Pull Request from `development` → `main` for production changes
- CI check (dotnet build) MUST pass before a PR can be merged
- NEVER commit `appsettings.json` if it contains real connection strings or secrets
- NEVER commit `bin/`, `obj/`, `.vs/`, `*.user` files — always `.gitignore` first
- ALWAYS run `git status` before committing — verify no unintended files are staged
- Branch protection on `main` is mandatory — always configured automatically via `gh api`
- Commit messages must be meaningful — never "fix", "update", or "changes"
- Auto-merge is ENABLED — PR merges to `main` automatically once CI passes (and approval if required)
- GitHub CLI (`gh`) is REQUIRED for all automation — install once, authenticate once
- On Windows: ALWAYS prefix `gh` commands with `export PATH="$PATH:/c/Program Files/GitHub CLI" &&`
- GitHub does NOT allow a PR author to approve their own PR — never set required approvals for sole developers

---

## BRANCH STRATEGY

| Branch        | Purpose                            | Who pushes            |
|---------------|------------------------------------|-----------------------|
| `main`        | Production — stable, protected     | Auto-merge via GitHub |
| `development` | Active development — all AI pushes | Developer / AI        |

---

## HOW TO USE THIS BOILERPLATE
<!-- FOR HUMAN REFERENCE ONLY — AI must ignore this section and follow EXECUTION RULES below -->

### Trigger A — One-Time Repository Setup
```
Using ai-github-ci-boilerplate.md, perform one-time repository setup:
GitHub Repo URL : https://github.com/{username}/{repo}.git
Solution Folder : {absolute path to solution root}
```

### Trigger D — Ship Changes (DEFAULT for daily use)
Just say any of:
```
Push my changes / Deploy / Ship it / Push and raise PR
```
> AI reads the diff, writes commit message, pushes to development, and opens/updates the PR — all automatically. No Trigger B or Trigger C needed separately.

### Trigger B — Push to Development only (rarely needed)
```
Using ai-github-ci-boilerplate.md, push my latest changes to development:
Commit Message : {describe what changed}
```

### Trigger C — Open PR only (rarely needed)
```
Using ai-github-ci-boilerplate.md, open a PR from development to main:
PR Title       : {short title}
PR Description : {what changed and why}
```

---

## EXECUTION RULES — AI MUST FOLLOW THESE EXACTLY

### TRIGGER A — ONE-TIME SETUP EXECUTION

Run all steps in this exact order. Do not skip any step.

**Step 1 — Install and authenticate GitHub CLI**
```bash
export PATH="$PATH:/c/Program Files/GitHub CLI"
gh auth status
```
- If `gh` not found: run `winget install --id GitHub.cli --silent --accept-package-agreements --accept-source-agreements` then re-check PATH
- If not authenticated: tell developer to open a terminal and run `gh auth login` → select GitHub.com → HTTPS → Login with a web browser. STOP and wait until developer confirms login is done.

**Step 2 — Verify git identity**
```bash
git config --global user.name
git config --global user.email
```
- If either is empty: STOP and ask developer for their name and GitHub email before continuing.

**Step 3 — Check appsettings.json for real secrets**
- Read `appsettings.json` — if ConnectionStrings or passwords contain real values (not placeholders like YOUR_SERVER): add `appsettings.json` to `.gitignore` and warn developer.
- If only placeholders: safe to commit, no action needed.

**Step 4 — Write .gitignore**
Write this file to the solution root before staging anything:
```
## .NET
bin/
obj/

## Visual Studio
.vs/
*.user

## Secrets
appsettings.local.json
*.env

## OS
.DS_Store
Thumbs.db
```

**Step 5 — Initialize git, commit, and push to main**
```bash
cd "{solution-folder}"
git init
git add .
git status        # verify — abort if bin/ obj/ .vs/ files appear
git commit -m "Initial commit"
git branch -M main
git remote add origin {GitHub Repo URL}
git push -u origin main
```

**Step 6 — Set main as the default branch on GitHub**
```bash
export PATH="$PATH:/c/Program Files/GitHub CLI"
gh api repos/{owner}/{repo} --method PATCH --field default_branch=main
```
> This prevents GitHub from setting `development` as the default branch later.

**Step 7 — Create and push development branch**
```bash
git checkout -b development
git push -u origin development
```

**Step 8 — Write CI workflow file**
Write `.github/workflows/ci.yml`:
```yaml
name: CI Build

on:
  push:
    branches:
      - development
  pull_request:
    branches:
      - main

jobs:
  build:
    name: Build
    runs-on: ubuntu-latest

    steps:
      - name: Checkout code
        uses: actions/checkout@v4

      - name: Setup .NET
        uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'

      - name: Restore dependencies
        run: dotnet restore

      - name: Build
        run: dotnet build --no-restore --configuration Release
```

**Step 9 — Commit and push CI workflow to development**
```bash
git add .github/workflows/ci.yml
git commit -m "Add GitHub Actions CI workflow"
git push origin development
```

**Step 10 — Ask developer: sole developer or team?**
Ask: "Is there a manager or reviewer who will approve PRs, or are you the only developer?"
- Store the answer — it determines branch protection in Step 11.
- NOTE: GitHub never allows a PR author to approve their own PR. Setting required approvals for a sole developer permanently blocks all PRs.

**Step 11 — Configure GitHub repo settings via gh CLI**
```bash
export PATH="$PATH:/c/Program Files/GitHub CLI"

# Enable auto-merge on the repo
gh api repos/{owner}/{repo} \
  --method PATCH \
  --header "Accept: application/vnd.github+json" \
  --field allow_auto_merge=true \
  --field allow_merge_commit=true
```

If **sole developer** (no reviewer) — CI only, no approval:
```bash
gh api repos/{owner}/{repo}/branches/main/protection \
  --method PUT \
  --header "Accept: application/vnd.github+json" \
  --input - <<'EOF'
{
  "required_status_checks": { "strict": false, "contexts": ["Build"] },
  "enforce_admins": false,
  "required_pull_request_reviews": null,
  "restrictions": null
}
EOF
```

If **team setup** (reviewer exists) — CI + 1 approval:
```bash
gh api repos/{owner}/{repo}/branches/main/protection \
  --method PUT \
  --header "Accept: application/vnd.github+json" \
  --input - <<'EOF'
{
  "required_status_checks": { "strict": false, "contexts": ["Build"] },
  "enforce_admins": false,
  "required_pull_request_reviews": {
    "required_approving_review_count": 1,
    "dismiss_stale_reviews": false
  },
  "restrictions": null
}
EOF
```

**Step 12 — Verify and report**
Run the verification checklist for Trigger A (see below). Report a summary table to the developer.

---

### TRIGGER D — SHIP CHANGES EXECUTION (Push + PR in one shot)

> Triggered by: "push my changes", "deploy", "ship it", "push and raise PR", or any similar phrase.
> NEVER ask developer to run Trigger B then Trigger C separately — always do both automatically.

**Step 1 — Read the diff and auto-generate commit message**
```bash
git diff
git diff --cached
git status
```
- Read all changes. Write a meaningful commit message that describes what changed — never ask the developer for it.
- If nothing is changed (`git status` is clean): skip to Step 4 (PR check only).

**Step 2 — Stage, verify, commit, push**
```bash
git checkout development
git add .
git status        # verify — abort if bin/ obj/ .vs/ files appear
git commit -m "{auto-generated meaningful message}"
git push origin development
```

**Step 3 — Check for existing open PR**
```bash
export PATH="$PATH:/c/Program Files/GitHub CLI"
gh api repos/{owner}/{repo}/pulls?state=open --jq '.[] | select(.head.ref=="development" and .base.ref=="main") | {number: .number, title: .title}'
```
- If an open PR from `development` → `main` already exists: note its number, skip to Step 5.
- If no open PR exists: proceed to Step 4.

**Step 4 — Create PR (only if none exists)**
```bash
export PATH="$PATH:/c/Program Files/GitHub CLI"
gh pr create --base main --head development \
  --title "{auto-generated title from diff}" \
  --body "{auto-generated description from diff}"
```
- Note the PR number from the output URL.

**Step 5 — Enable auto-merge on the PR**
```bash
export PATH="$PATH:/c/Program Files/GitHub CLI"
gh pr merge {PR number} --auto --merge
```

**Step 6 — Report summary**
Show a single table: files committed, commit SHA, PR number/link, auto-merge status, CI status.

---

### TRIGGER B — PUSH TO DEVELOPMENT ONLY

**Step 1 — Stage, verify, commit, push**
```bash
git checkout development
git add .
git status        # verify — abort if bin/ obj/ .vs/ files appear
git commit -m "{Commit Message from developer or auto-derived from diff}"
git push origin development
```

---

### TRIGGER C — OPEN PR ONLY

**Step 1 — Verify development is clean**
```bash
git checkout development
git status        # must be clean — nothing uncommitted
```

**Step 2 — Check for existing open PR**
```bash
export PATH="$PATH:/c/Program Files/GitHub CLI"
gh api repos/{owner}/{repo}/pulls?state=open --jq '.[] | select(.head.ref=="development" and .base.ref=="main") | {number: .number}'
```
- If PR already exists: enable auto-merge on it and report — do NOT create a duplicate.
- If no PR exists: create one then enable auto-merge.

**Step 3 — Create PR if needed**
```bash
gh pr create --base main --head development --title "{PR Title}" --body "{PR Description}"
gh pr merge {PR number} --auto --merge
```

---

## CODE GENERATION RULES — MANDATORY

1. NEVER output git commands or YAML in chat as the final response — always execute them directly via bash tools.
2. ALWAYS write `.gitignore` and `.github/workflows/ci.yml` using file write tools — never echo/heredoc in bash.
3. ALWAYS run `gh api` commands directly — never instruct the developer to configure GitHub settings manually in the browser.
4. ALWAYS prefix `gh` commands with `export PATH="$PATH:/c/Program Files/GitHub CLI" &&` on Windows.
5. ALWAYS treat "push my changes", "deploy", "ship it" as Trigger D — never split into B then C.
6. ALWAYS auto-derive commit message and PR title from `git diff` — never ask the developer to provide them in Trigger D.
7. ALWAYS check for an existing open PR before creating a new one — never create duplicate PRs.
8. ALWAYS set `default_branch=main` via `gh api` during Trigger A — prevents GitHub from defaulting to `development`.
9. Show only a short summary table at the end of each trigger.

---

## VERIFICATION CHECKLIST

### After Trigger A:
- [ ] `gh auth status` confirms logged in
- [ ] `git config user.name` and `user.email` are set
- [ ] `.gitignore` exists — excludes `bin/`, `obj/`, `.vs/`, `*.user`
- [ ] `git remote -v` shows correct GitHub URL
- [ ] `main` is the default branch on GitHub
- [ ] `main` branch pushed with all solution files
- [ ] `development` branch pushed to GitHub
- [ ] `.github/workflows/ci.yml` committed and pushed to `development`
- [ ] `allow_auto_merge: true` confirmed via `gh api`
- [ ] Branch protection on `main` confirmed — CI required; approval only if reviewer exists

### After Trigger D / Trigger B:
- [ ] `git status` is clean after push
- [ ] No `bin/`, `obj/`, `.vs/` files committed
- [ ] CI workflow triggered on GitHub Actions

### After Trigger D / Trigger C:
- [ ] PR is open from `development` → `main`
- [ ] No duplicate PRs created
- [ ] Auto-merge enabled on the PR
- [ ] CI check running or passing on the PR
