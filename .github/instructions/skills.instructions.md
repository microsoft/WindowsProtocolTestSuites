---
applyTo: '**'
---
## Skills Overview

Skills in this repository provide domain-specific expertise for writing test cases and protocol implementations. Each skill contains detailed guidance and references for working with specific protocols or test suites.

### Available Skills

Skills are located in `.github/skills/`. Each subdirectory represents a skill with its own `SKILL.md` file containing detailed instructions. To discover available skills, list the contents of `.github/skills/` directory.

### How to Use Skills

When working on a task that relates to a specific domain:
1. **Discover skills** - List the `.github/skills/` directory to see all available skills
2. **Load the skill** - Read the `SKILL.md` file from the relevant skill directory (e.g., `.github/skills/<skill-name>/SKILL.md`)
3. **Reference specific aspects** - Each skill contains high-level guidance and detailed references in its `references/` subdirectory
4. **Discover reusable components** - Skills guide you to existing adapters, base classes, and ProtoSDK libraries

### Skill Structure

Each skill directory contains:
- `SKILL.md` - Main guidance with metadata and actionable instructions
- `references/` - Detailed protocol-specific documentation loaded on-demand

Skills use progressive disclosure to keep the context window efficient:
- **Metadata** (always available) - Name and description
- **Instructions** (loaded when skill is relevant) - Patterns and guidance
- **References** (loaded as needed) - Detailed specs and examples
- **Scripts** (loaded as needed) - Code to be run for specific tasks