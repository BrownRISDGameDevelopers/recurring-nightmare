# recurring-nightmare

## Working with Git
Some shorthand words to know
- repo: repository. Essentially our project folder
- remote: The remote repository. In our case, this means the recurring-nightmare repo uploaded on github
- local: The local repository. It is the project folder on your own personal computer (local machine)
- commit: A group of changes. Usually, I recommend doing "atomic commits" meaning make one commit be an independent group of changes. 
For example, you shouldn't have a commit that changes the user behavior AND changes enemy spawn behavior. These should be 2+ commits.
If you have changes that should belong in separate commits, you can specify which changes to add for this specific commit using `git add <filename>`.

### Implementing Changes
If you want to implement your changes to the main branch, follow these steps.
1. Sync you local main branch (not the main branch on github) is up to date with remote.

You can move to your main branch by running `git checkout main`. Then, run `git pull`.

2. Create a new branch by running `git checkout -b <branch-name>`. 

For example, `git checkout -b example-branch`

3. Add and commit changes to the new branch.

You can select specific changes you want to add to the commit by `git add <specific-file-name>` or you can commit all changes by `git add .` 
Then, commit the changes with a meaningful message like `git commit -m "fix player jump animation bug"`. 
I cannot emphasize the importance of a meaningful message. Please no "lol" "amongus".

4. Publish branch to remote with `git push -u origin <name-of-branch>`.

5. Make a pull request on github.com
