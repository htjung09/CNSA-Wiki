window.triggerFileInput = (element) => {
    element.click();
};

window.appendToTextarea = (id, text) => {
    const el = document.getElementById(id);
    el.value += text;
};