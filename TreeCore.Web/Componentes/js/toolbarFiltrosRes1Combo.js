
// #region CONTROL RESPONSIVE TOOLBAR 2 COMBOS


function StyleOnResize(sender) {
    var tbFiltros = document.getElementsByClassName("tlbGridRes");
    var ruta = getIdComponente(sender);

    //CONTROL PARA CUANDO NO HAYA BOTONES, SOLO COMBO FILTROS
    var GrupoBtnFilters = document.getElementsByClassName("GrupoBtnFilters");
    var BotonesVisibles = GrupoBtnFilters[0].style.display;

    var cmbMisfiltros = document.getElementsByClassName("cmbMisfiltros");

    if (BotonesVisibles == "none") {
        cmbMisfiltros[0].classList.add("cmbMisfiltrosNoBtns");
    }

    // ------------------------------------------

    for (i = 0; i < tbFiltros.length; i++) {

        if (tbFiltros[i] != null) {

            if (tbFiltros[i].clientWidth < 612) {

                tbFiltros[i].classList.add("tlbGridResMid");

            }
            else {

                tbFiltros[i].classList.remove("tlbGridResMid");


            }


            if (tbFiltros[i].clientWidth < 460) {

                tbFiltros[i].classList.add("tlbGridResMini");

            }
            else {
                tbFiltros[i].classList.remove("tlbGridResMini");


            }
            App[ruta + '_' + 'tbFiltros'].updateLayout();


        }


    }


}


// #endregion
