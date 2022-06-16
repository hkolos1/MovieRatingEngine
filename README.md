<img height="300" src="https://i.ibb.co/VCPHpcg/Logo-vertical.png">

## Introduction

Goal is to build a movie rating engine, similar to the one on imdb.com, but much, much 
simpler and more rudimentary. So we would not waste time, your solution does not have 
to be nearly as detailed. Each movie should only have a title, a cover image, a 
description, release date, and cast (at least two actors per movie). Also, every movie can 
be rated by other users by 1-5 stars.

## User Stories

- Upon starting the app, user should be presented with the 10 top rated movies
- There should be a tab, or a toggle switch that would allow the user to see the top rated 10 
TV Shows
- Above the Toggle/Tab component, there should be a search bar that would allow user to 
search for any movie/show in the DB
- Search bar should react to user's input automatically, but not before there's at least 2 
characters entered in the search bar
- Search should be by any of the movie's textual attributes
- Search engine should also understand phrases like "5 stars", "at least 3 stars", "after 2015", 
"older than 5 years"
- Search results should still be sorted by movie rating, just as they are upon the app start (so 
the page always displays either top 10 rated movies of all time, or top 10 rated movies among 
all the search results)
- If the search bar is cleared, top 10 movies/shows of all time should reappear as results, 
depending on which tab/toggle switch value is selected
- Implement the "view more results" feature, which would, every time the user clicks on it, 
load 10 more results.
- Also implement a simple list of all the movies where users can give their rating to each 
movie. No need for textual feedback, only star-rating will be enough. You don't need to 
authenticate those users, anonymous rating will suffice
- Every movie in the list mentioned above should have a graphic control that displays the 
average rating of the particular movie

## Instructions

- Implement some kind of API authentication so no unauthorized requests would be 
responded to from the API side.
- Store the movie images any way you prefer or find the best suited for this purpose (A file on 
the file system, BLOB in the DB, some kind of cloud service like AWS S3, or something 
completely different)
- Make sure your API follows REST principles
- Make sure that you white list all the REST verbs used in your API

## Technologies

- ASP .NET 5.0 Web API, Microsoft SQL Server, JWT, Postman
