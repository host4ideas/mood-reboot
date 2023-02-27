// Chat button
function buttonHandler() {
    $("#chatWindow").fadeToggle("fast");
}

$(function () {
    $("#chatWindow").resizable({
        maxHeight: 250,
        maxWidth: 350,
        minHeight: 150,
        minWidth: 200
    });
});

// Scroll to top button
// Get the button 
let mybutton = document.getElementById('btn-back-to-top'); // When the user scrolls down 20px from the top of the document, show the button
window.onscroll = function () { scrollFunction(); };
function scrollFunction() {
    console.log("test")
    if (document.body.scrollTop > 20 ||
        document.documentElement.scrollTop > 20) {
        mybutton.style.display =
            'flex';
    } else {
        mybutton.style.display = 'none';
    }
}
// When the user clicks on the button, scroll to the top of the document
mybutton.addEventListener('click', backToTop);
function backToTop() {
    document.body.scrollTop = 0; document.documentElement.scrollTop = 0;
}

// Theme toggle
var themeToggleDarkIcon = document.getElementById('theme-toggle-dark-icon');
var themeToggleLightIcon = document.getElementById('theme-toggle-light-icon');         // Change the icons inside the button based on previous settings
if (localStorage.getItem('color-theme') === 'dark' || (!('color-theme' in localStorage) && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
    themeToggleLightIcon.classList.remove('hidden');
} else {
    themeToggleDarkIcon.classList.remove('hidden');
} var themeToggleBtn = document.getElementById('theme-toggle'); themeToggleBtn.addEventListener('click', function () {             // toggle icons inside button
    themeToggleDarkIcon.classList.toggle('hidden');
    themeToggleLightIcon.classList.toggle('hidden');             // if set via local storage previously
    if (localStorage.getItem('color-theme')) {
        if (localStorage.getItem('color-theme') === 'light') {
            document.documentElement.classList.add('dark');
            localStorage.setItem('color-theme', 'dark');
        } else {
            document.documentElement.classList.remove('dark');
            localStorage.setItem('color-theme', 'light');
        }                 // if NOT set via local storage previously
    } else {
        if (document.documentElement.classList.contains('dark')) {
            document.documentElement.classList.remove('dark');
            localStorage.setItem('color-theme', 'light');
        } else {
            document.documentElement.classList.add('dark');
            localStorage.setItem('color-theme', 'dark');
        }
    }
});

// Leave a message script
const formWrapper = document.querySelector(".formbold-form-wrapper");
const crossIcon = document.querySelector(".cross-icon");
const chatIcon = document.querySelector(".chat-icon");
function chatboxToogleHandler() {
    formWrapper.classList.toggle("hidden");
    crossIcon.classList.toggle("hidden");
    chatIcon.classList.toggle("hidden");
}