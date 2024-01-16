document.querySelectorAll(".select-list").forEach(selectList => {
    const selectedOption = selectList.querySelector("input");
    selectedOption.addEventListener("click", () => selectList.classList.toggle("expanded"));
    const selectableOptions = selectList.querySelectorAll("span.option");
    for (let i = 0; i < selectableOptions.length; i++) {
        const option = selectableOptions[i];
        option.style.top = ((i+1) * 30) + "px"; // todo: automate this part with react components
        option.addEventListener("click", () => {
            selectedOption.value = option.dataset.value;
            selectList.classList.remove("expanded");
        });
    };
});