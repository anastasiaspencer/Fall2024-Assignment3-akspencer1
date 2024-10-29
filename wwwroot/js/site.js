// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function goToCreateView() {
    window.location.href = '/Movies/Create';
}

function goToDeleteView() { 
    window.location.href = '/Movies/Delete';
}

function goToEditView() {
    window.location.href = '/Movies/Edit';
}

function goToCreateActorView() {
    window.location.href = '/Actors/Create';
}

function goToEditActorView() {
    window.location.href = '/Actors/Edit';
}

function goToDeleteActorView() {

    window.location.href = '/Actors/Delete';
}

function goToAddActorsView() {
    window.location.href = '/Movies/AddActors';
}

function goToManageMovieView() {
    window.location.href = '/Actors/AddMovies';
}

function goToGenerateReviews() {
    window.location.href = '/AI/GenerateReviews';
}

function goToGenerateTweets() {
    window.location.href = '/AI/GenerateTweets';
}

function goToDetailsView(movieTitle) {
    //This will redirect to the Details page with the movie table passed in so the right details page is rendered
    window.location.href = `Movies/Details?title=${encodeURIComponent(movieTitle)}`;
}