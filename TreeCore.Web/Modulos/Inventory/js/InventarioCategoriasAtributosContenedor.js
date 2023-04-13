function NavegacionTabs(sender) {
    if (App.ctMain1.hidden) {
        App.ctMain1.show();
        App.ctMain2.hide();
    } else {
        App.ctMain1.hide();
        App.ctMain2.show();
    }
}