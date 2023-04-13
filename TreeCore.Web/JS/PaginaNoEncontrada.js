function openGlobal() {
    if (parent != window) {
        parent.openGlobal();
    }
    else {
        window.location.replace("/");
    }
}