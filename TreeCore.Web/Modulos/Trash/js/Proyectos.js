
function CambiarVista(vista) {
    let iframe = "";
    App.ctMain1.hide();
    App.ctMain2.hide();
    App.ctMain3.hide();
    switch (vista) {
        case 1:
            App.ctMain1.show();
            document.getElementById("lnkProyectos").lastChild.classList.add("navActivo");
            document.getElementById("lnkProyectosSLA").lastChild.classList.remove("navActivo");
            document.getElementById("lnkProyectosUsuarios").lastChild.classList.remove("navActivo");
            App.btnCollapseAsRClosed.hide();
            break;
        case 2:
            document.getElementById("lnkProyectos").lastChild.classList.remove("navActivo");
            document.getElementById("lnkProyectosUsuarios").lastChild.classList.remove("navActivo");
            document.getElementById("lnkProyectosSLA").lastChild.classList.add("navActivo");
            App.btnCollapseAsRClosed.hide();
            App.ctMain2.show();
            break;
        case 3:
            document.getElementById("lnkProyectos").lastChild.classList.remove("navActivo");
            document.getElementById("lnkProyectosSLA").lastChild.classList.remove("navActivo");
            document.getElementById("lnkProyectosUsuarios").lastChild.classList.add("navActivo");
            if (App.btnCollapseAsRClosed.Hidden == false) {
                App.btnCollapseAsRClosed.show();
            }
            
            App.ctMain3.show();
            break;
        default:
            App.ctMain1.show();
            App.btnCollapseAsRClosed.hide();
    }
}