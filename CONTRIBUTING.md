# :pencil2: Bluechirp Contributing Guidelines
This document will aid you in the case that you want to open a pull request for this project.  
Your help is appreciated! :blush:

## :thinking: Starting
Please open a draft pull request or tell others that you will work on an issue before you spend too much time into it.

Sometimes, someone else may also try to fix the issue you're trying to fix, which will cause both of you to end up wasting time.  
Communication is key!

Also remember to read the [code of conduct](CODE_OF_CONDUCT.md)!

## :incoming_envelope: Commit Messages
Follow this format! It makes commit messages much nicer.

> [PROJECT -> MODULE] COMMIT_MESSAGE

Here's a list of what you should replace PROJECT with:
- :iphone: Bluechirp.csproj - **App**
- :link: Bluechirp.Library.csproj - **Library**
- :white_check_mark: Bluechirp.Tests.csproj - **Tests**
- :mag: MastoParserLib.csproj - **Parser**
- :card_index_dividers: Repository - **GitHub**

MODULE should be the folder the modified files are in, like "Core" or "Model".  
For GitHub though, here is the list:
- :robot: Bots - **Bots**
- :arrow_forward: Actions - **Actions**
- :question: Other - **General**

If you are committing for the Tests/Parser project, you do not need to include the MODULE part.

## :bar_chart: Indentation Style
The indentation style must be [Allman](https://en.wikipedia.org/wiki/Indentation_style#Allman_style).

This is because it looks cleaner, since the spacing is much more noticeable and because most C# projects use it anyways.

## :wind_face: Spacing
You must leave 1 new line between each function/class/struct/statement.

You may group a statement (i.e. an `if` statement) group if it makes sense to do so. For example, if you are checking several cases against one variable, or it contextually makes sense to place them together.

Please don't leave extra random empty lines at the end or start of functions and such!

## :label: Naming
Use `PascalCase` for class names, variables and parameters, but use `camelCase` for local variables. Use `SCREAMING_SNAKE_CASE` for const variables.

Anything private must use the `_PascalCase` or `_SCREAMING_SNAKE_CASE` convention.

## :mega: Comments
Please use XML documentation to document all classes and functions.

Don't comment over-comment! It can have the opposite effect and make code annoying to read.

## :card_file_box: Types
Please use the keyword version of types when possible, this means using `string` instead of `String` (looking at you, pesky Java users), `int` instead of `Int32`, `uint` instead of `UInt32` and so on.

Do the exact opposite when working with binary files, where knowing the datatype in detail is crucial.
