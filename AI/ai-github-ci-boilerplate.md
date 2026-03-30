# GITHUB CI MASTER BOILERPLATE
# For AI-Driven GitHub Setup & Continuous Integration — Follow ALL rules strictly

---

## STRICT RULES

- NEVER push directly to `main` — all changes go to `development` first
- ALWAYS create a Pull Request from `development` → `main` for production changes
- CI check (dotnet build) MUST pass before a PR can be merged
- NEVER commit `appsettings.json` if it contains real connection strings or secrets
- NEVER commit `bin/`, `obj/`, `.vs/`, `*.user` files — always create `.gitignore` first
- ALWAYS check `git status` before committing — never commit unintended files
- ALWAYS verify `git config user.name` and `git config user.email` before first commit
- Branch protection on `main` is mandatory — enforced via GitHub branch rules
- Commit messages are free-form but must be meaningful (not "fix" or "update")
- Auto-merge is ENABLED — manager only needs to Approve, GitHub merges automatically
- Manager does NOT need to click Merge — approval alone triggers the merge
- Repository MUST be Private for work/enterprise projects — warn user if Public

---

## BRANCH STRATEGY

| Branch        | Purpose                          | Who pushes       |
|---------------|----------------------------------|------------------|
| `main`        | Production — stable, reviewed    | Manager (merge)  |
| `development` | Active development — all pushes  | Developer / AI   |

---

## HOW TO USE THIS BOILERPLATE
<!-- FOR HUMAN REFERENCE ONLY — AI: ignore this section -->

### Trigger A — One-Time Repository Setup (run once per project)
```
Using ai-github-ci-boilerplate.md, perform one-time repository setup:
GitHub Repo URL : https://github.com/{username}/{repo}.git
Solution Folder : {absolute path to .sln or .csproj root}
```

### Trigger B — Push Changes to Development
```
Using ai-github-ci-boilerplate.md, push my latest changes to development:
Commit Message  : {describe what changed}
```

### Trigger C — Open PR from Development to Main
```
Using ai-github-ci-boilerplate.md, open a PR from development to main:
PR Title        : {short title}
PR Description  : {what this change does and why}
```

---

## PATTERN A — ONE-TIME REPOSITORY SETUP

> Run this once when the project has no git history and the GitHub repo is already created.

### Step 0 — Pre-flight checks (MANDATORY before anything else)

**Check 1 — Git identity configured?**
```bash
git config --global user.name
git config --global user.email
```
> If either returns empty, set them before proceeding:
> ```bash
> git config --global user.name "Developer Name"
> git config --global user.email "developer@email.com"
> ```
> STOP and ask the user for their name and GitHub email if not set.

**Check 2 — Repo visibility warning**
> After setup, remind the user:
> "Your GitHub repo is currently PUBLIC. For work projects, go to:
> GitHub → your repo → Settings → scroll to bottom → Change repository visibility → Make private"

**Check 3 — Secrets in appsettings.json**
> Check if `appsettings.json` contains ConnectionStrings or any passwords.
> If yes, add `appsettings.json` to `.gitignore` and warn the user:
> "appsettings.json contains secrets and has been excluded from git.
> Store real connection strings in environment variables or Azure Key Vault."

---

### Step 1 — Create .gitignore
Write the file `.gitignore` in the solution root before staging anything:
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

### Step 2 — Initialize git and push to main
```bash
cd "{solution-folder}"
git init
git add .
git status                              # verify — confirm no bin/obj/.vs files staged
git commit -m "Initial commit"
git branch -M main
git remote add origin {GitHub Repo URL}
git push -u origin main
```
> On first push a browser window or credential popup will appear.
> The developer must log in to GitHub in that window.
> Git Credential Manager stores the token — no login needed on future pushes.

### Step 3 — Create development branch
```bash
git checkout -b development
git push -u origin development
```

### Step 4 — Write GitHub Actions CI workflow
Write the file `.github/workflows/ci.yml` (see CI WORKFLOW section below).

### Step 5 — Commit and push the CI workflow
```bash
git add .github/workflows/ci.yml
git commit -m "Add GitHub Actions CI workflow"
git push origin development
```

### Step 6 — Instruct user to configure GitHub repo settings
> AI cannot change GitHub settings without GitHub CLI.
> After setup, tell the user exactly:
>
> **Part 1 — Enable Auto-Merge (so manager approval auto-merges the PR):**
> 1. Go to your GitHub repo → click Settings tab
> 2. Scroll down to the Pull Requests section
> 3. Check: Allow auto-merge
> 4. Click Save
>
> **Part 2 — Set Branch Protection on main:**
> 1. In the left sidebar click Branches
> 2. Under Branch protection rules click Add rule
> 3. In Branch name pattern type: main
> 4. Check: Require a pull request before merging
> 5. Check: Require approvals → set to 1
> 6. Check: Require status checks to pass before merging
>    → In the search box type 'Build' and select it
> 7. Click Create (green button at bottom)
>
> Once both are done:
> - Manager receives a PR notification
> - Manager reviews the code and clicks Approve
> - GitHub automatically merges to main — no extra click needed from manager"

---

## PATTERN B — PUSH CHANGES TO DEVELOPMENT

> Run this every time the developer wants to push new work.

### Step 1 — Verify git identity
```bash
git config --global user.name
git config --global user.email
```
> If empty, stop and ask the user to provide name and email before continuing.

### Step 2 — Stage, verify, commit, push
```bash
git checkout development
git add .
git status                              # verify files — never commit bin/obj/.vs
git commit -m "{Commit Message}"
git push origin development
```

### Step 3 — Report CI status
After push:
- GitHub automatically triggers the CI workflow (dotnet build)
- If CI passes → green check on the commit
- If CI fails → red check — AI must read the error, fix the code, and push again

---

## PATTERN C — OPEN PR FROM DEVELOPMENT TO MAIN

> Run this when development is stable and ready for manager review.

### Step 1 — Verify development is clean and pushed
```bash
git checkout development
git status                              # must be clean — no uncommitted changes
```

### Step 2 — Open PR with Auto-Merge enabled
Instruct the user to go to GitHub and open a PR:
```
GitHub → your repo → Pull Requests tab → New Pull Request
Base    : main
Compare : development
Title   : {PR Title}
Body    : {PR Description — what changed and why}
Click: Create Pull Request
```

After PR is created, instruct user to enable auto-merge:
```
On the PR page → click "Enable auto-merge" button
Select: Merge commit
Click: Confirm auto-merge
```

> OR if gh CLI is available (does both in one command):
```bash
gh pr create --base main --head development --title "{PR Title}" --body "{PR Description}"
gh pr merge --auto --merge
```

### Step 3 — Notify manager
After PR is opened with auto-merge enabled, tell the user:
> "PR is open and auto-merge is enabled.
> Your manager just needs to review the code and click Approve.
> GitHub will automatically merge to main — manager does NOT need to click Merge."

After manager approves:
- GitHub auto-merges `development` → `main`
- CI runs on `main` to confirm merged code is clean

---

## CI WORKFLOW — GitHub Actions

### File: .github/workflows/ci.yml
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

> This workflow runs on every push to `development` and on every PR targeting `main`.
> If `dotnet build` fails, the CI check fails and the PR cannot be merged.

---

## GENERATION ORDER — MANDATORY

### For Trigger A (One-Time Setup):
1. Check `git config user.name` and `user.email` — stop if not set, ask user
2. Warn if repo is Public — remind user to make it Private
3. Check `appsettings.json` for secrets — add to `.gitignore` if found
4. Write `.gitignore` into solution root
5. `git init` + `git add .` + `git status` (verify) + `git commit` + `git push` to `main`
6. Create and push `development` branch
7. Write `.github/workflows/ci.yml`
8. Commit and push the workflow file to `development`
9. Give user exact branch protection steps to follow on GitHub

### For Trigger B (Push Changes):
1. Check `git config user.name` and `user.email` — stop if not set
2. `git checkout development`
3. `git add .` + `git status` (verify no bin/obj/.vs files)
4. `git commit -m "{message}"`
5. `git push origin development`
6. Confirm CI triggered — report pass or fix failures

### For Trigger C (Open PR):
1. Verify `git status` is clean on development
2. Instruct user to open PR via GitHub website (or use `gh pr create` if available)
3. Confirm PR is open and tell user manager has been notified

---

## CODE GENERATION RULE — MANDATORY

NEVER show git commands or YAML in the chat as the final output.
ALWAYS execute git commands directly via bash tools.
ALWAYS write `.gitignore` and `.github/workflows/ci.yml` directly using file write tools.
Show only a short summary table of actions taken at the end.

---

## VERIFICATION CHECKLIST

### After Trigger A:
- [ ] `git config user.name` and `user.email` are set
- [ ] `.gitignore` exists and excludes `bin/`, `obj/`, `.vs/`, `*.user`
- [ ] `git remote -v` shows the correct GitHub URL
- [ ] `main` branch exists on GitHub with all solution files
- [ ] `development` branch exists on GitHub
- [ ] `.github/workflows/ci.yml` is committed and pushed to `development`
- [ ] User warned about Public repo visibility
- [ ] User given exact branch protection steps
- [ ] User informed about first-push authentication popup

### After Trigger B:
- [ ] `git status` is clean after push
- [ ] No `bin/`, `obj/`, or `.vs/` files were committed
- [ ] CI workflow triggered on GitHub (green or in-progress)

### After Trigger C:
- [ ] PR is open from `development` → `main`
- [ ] Auto-merge is enabled on the PR
- [ ] CI check is passing on the PR
- [ ] Manager has been notified — they only need to Approve, not click Merge
