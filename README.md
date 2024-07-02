# Coding tracker
A console app, it helps to track the amount of time you spent coding (or doing any activity really), is part of my journey learning C# through projects.

## Description:
- As I stated previously, this project is mainly written in C# as a console app, it also uses SQLite with Dapper and the Spectre Console library to manage the CLI.

- This app has the 4 main CRUD functionalities, you can add a session manually or start one in the moment. All info is stored in a database file and you can easily access it through the "View All" function. Of course you can alter said info by deleting or updating some records.

- It also comes with a simple report functionality that shows you your total and average time per week and month.

- At the moment of writing this I'm currently implementing a function to work with goals but it's not ready yet.
  
- I tried doing a filter function to filter based in some sort of time period but I had some problems thinking on how to manage the info so I decided to postpone said function (not really sure if I'm going to do it though)

## Difficulties:
- Throughout the whole project I encountered several difficulties, mainly related to the manipulation of the info in the database, I had to search and read carrefully every time I encountered an error.
- At first, I kinda couldn't figure out how to use the spectre console library so I had to read very carefully the docs but once I did i quickly got the hang of it.
- Also, the option to start a session really gave me headaches, I tried really hard to do a proper stopwatch but it turns out it's pretty difficult to do one that works as intended in a console app so I gave up and made the closest thing that I could


## Tasks
- [x] Add the "View All" option
- [x] Add the "Update" option
- [x] Add the "Delete" option
- [x] Add the "Add session" option
- [x] Add the "Start new session now" option
- [x] Add the report function
- [ ] \(Optional) Add the filter function (probably won't lol)
- [ ] Add the goals system
