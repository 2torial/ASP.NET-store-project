document.querySelectorAll(".drop-down-section").forEach(section => {
    const label = section.querySelector(".drop-down-label");
    label.addEventListener("click", () => section.classList.toggle("expanded"));
});