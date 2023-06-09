#!/bin/bash

# Create directories.txt
echo "/SkillzBotHost" > directories.txt
echo "/volume/SkillzBot" >> directories.txt

# Run GigFilesChecker
chmod +x GigFilesChecker
./GigFilesChecker

# Start SkillzBot
chmod +x /volume/SkillzBot/SkillzBot
xvfb-run /volume/SkillzBot/SkillzBot