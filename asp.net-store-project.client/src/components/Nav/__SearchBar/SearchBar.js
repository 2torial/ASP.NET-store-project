document.querySelectorAll(".paginator").forEach(paginator => {
    const pageIndex = paginator.querySelector(".page-index");
    pageIndex.addEventListener("click", () => {
        pageIndex.classList.remove("idle");
        pageIndex.readOnly = false;
        pageIndex.select();
    });
    pageIndex.addEventListener("focusout", () => {
        const onlyPositiveNumbersPattern = /^[1-9][0-9]*/;
        if (!onlyPositiveNumbersPattern.test(pageIndex.value))
            pageIndex.value = pageIndex.dataset.currentPage;
        pageIndex.classList.add("idle");
        pageIndex.readOnly = true;
    });
});