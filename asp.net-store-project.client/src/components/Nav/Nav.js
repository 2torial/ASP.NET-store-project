searchBar = document.querySelector(".search-bar");
focusSearchBar = () => searchBar.classList.add("focused");
unfocusSearchBar = () => searchBar.classList.remove("focused");

searchBarInputArea = searchBar.querySelector(".input-area");
searchBarInputArea.addEventListener("focus", focusSearchBar);
searchBarInputArea.addEventListener("focusout", unfocusSearchBar);

menuArea = document.querySelector(".menu");
openMenu = () => menuArea.style.top = "60px";
hideMenu = () => menuArea.style.top = "7px";

menuSection = document.querySelector(".menu-section");
profileIcon = menuSection.querySelector("#profile-link");
profileIcon.addEventListener("mouseover", openMenu);

mainArea = document.querySelector("main");
mainArea.addEventListener("mouseover", hideMenu);