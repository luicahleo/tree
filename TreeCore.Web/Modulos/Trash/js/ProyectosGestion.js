var PuntoCorteL = 900;
var PuntoCorteS = 512;
var selectedCol = 0;
var isOnMobC = 0;


function ControlSlider() {
    var containerSize = parent.App.CenterPanelMain.getWidth();


    var pnmain = App.grdProjects;
    var col2 = App.pnCol1;
    var col3 = App.pnCol2;
    var tbsliders = App.tbSliders;
    var btnPrevSldr = App.btnPrevGrid;
    var btnNextSldr = App.btnNextGrid;


    //state 2 cols

    if (containerSize > PuntoCorteL) {
        pnmain.show();
        col2.show();
        col3.show();
        selectedCol = 1;

        isOnMobC = 0;

    }

    if (containerSize < PuntoCorteL && containerSize > PuntoCorteS) {
        pnmain.show();



        if (selectedCol == 3) {
            col2.hide();
            col3.show();
        }
        else {
            col2.show();
            col3.hide();
        }
        isOnMobC = 0;




    }


    // state 1 col
    if (containerSize < PuntoCorteS && isOnMobC == 0) {
        pnmain.show();
        col2.hide();
        col3.hide();

        btnPrevSldr.disable();
        btnNextSldr.enable();

        selectedCol = 1;

        isOnMobC = 1;
    }



    //CONTROL SHOW OR HIDE BOTONES SLIDER
    if (pnmain.hidden == true || col2.hidden == true || col3.hidden == true) {

        tbsliders.show();

        if (pnmain.hidden != true && col2.hidden == false && col3.hidden == true) {
            btnPrevSldr.disable();

        }

    }
    else {


        tbsliders.hide();
        btnPrevSldr.disable();
        btnNextSldr.enable();


    }

}

function SliderMove(NextOrPrev) {

    var containerSize = parent.App.CenterPanelMain.getWidth();

    var pnmain = App.grdProjects;
    var col1 = App.pnCol1;
    var col2 = App.pnCol2;
    var btnPrevSldr = App.btnPrevGrid;
    var btnNextSldr = App.btnNextGrid;

    //SELECCION EN 2  PANELES
    if (containerSize < PuntoCorteL && containerSize > PuntoCorteS) {

        if (NextOrPrev == 'Next') {
            col1.hide();
            col2.show();
            selectedCol = 3;

            btnPrevSldr.enable();
            btnNextSldr.disable();

        }
        else if (NextOrPrev == 'Prev') {

            col1.show();
            col2.hide();
            selectedCol = 2;

            btnPrevSldr.disable();
            btnNextSldr.enable();

        }
    }

    //SELECCION EN 1  PANEL
    else {

        if (NextOrPrev == 'Next' && selectedCol == 1) {
            pnmain.hide();
            col1.show();
            col2.hide();
            selectedCol = 2;

            btnPrevSldr.enable();
            btnNextSldr.enable();

        }
        else if (NextOrPrev == 'Prev' && selectedCol == 2) {
            pnmain.show();
            col1.hide();
            col2.hide();
            selectedCol = 1;

            btnPrevSldr.disable();
            btnNextSldr.enable();

        }
        else if (NextOrPrev == 'Next' && selectedCol == 2) {
            pnmain.hide();
            col1.hide();
            col2.show();
            selectedCol = 3;

            btnPrevSldr.enable();
            btnNextSldr.disable();

        }

        else if (NextOrPrev == 'Prev' && selectedCol == 3) {
            pnmain.hide();
            col1.show();
            col2.hide();
            selectedCol = 2;

            btnPrevSldr.enable();
            btnNextSldr.enable();

        }


    }


}

var handlePageSizeSelect = function (item, records) {
    var curPageSize = App.storeProyectos.pageSize,
        wantedPageSize = parseInt(item.getValue(), 10);

    if (wantedPageSize != curPageSize) {
        App.storeProyectos.pageSize = wantedPageSize;
        App.storeProyectos.load();
    }

}
var accion;
var indiceMaximo = 4;
var tabActual = 0;
function NavegacionTab(index) {
    if (index == 'btnNext') {
        tabActual++;
        index = tabActual;
    } else if (index == 'btnPrev') {
        tabActual--;
        index = tabActual;
    }
    
    
    
    if (index <= indiceMaximo) {
        switch (index) {
            case 0:
                App.pnFormProject.show();
                App.pnPrincipalFases.hide();
                App.pnPrincipalZonas.hide();
                App.pnPrincipalEmpresaProveedora.hide();
                App.pnProyectosTipos.hide();

                App.btnNext.setDisabled(false);
                App.btnPrev.setDisabled(true);
                App.btnAnadirProyecto.show();
                if (accion == 'agregar') {
                    App.winGestion.setTitle(jsAgregar + ' ' + jsProyecto);
                } else if (accion == 'editar') {
                    App.winGestion.setTitle(jsEditar + ' ' + jsProyecto);
                    App.lnkFases.enable();
                    App.lnkZonas.enable();
                    App.lnkEmpresaProveedora.enable();
                    App.lnkProyectosTipos.enable();

                } else if (accion == 'duplicar') {
                    App.winGestion.setTitle(jsDuplicar + ' ' + jsProyecto);
                }

                App.btnNext.hide();
                App.btnAnadirProyecto.show();


                document.getElementById("lnkProyectos").lastChild.classList.add("navActivo");
                document.getElementById("lnkFases").lastChild.classList.remove("navActivo");
                document.getElementById("lnkZonas").lastChild.classList.remove("navActivo");
                document.getElementById("lnkEmpresaProveedora").lastChild.classList.remove("navActivo");
                document.getElementById("lnkProyectosTipos").lastChild.classList.remove("navActivo");
                tabActual = 0;
                break;
            case 4:
                App.pnFormProject.hide();
                App.pnPrincipalFases.show();
                App.pnPrincipalZonas.hide();
                App.pnPrincipalEmpresaProveedora.hide();
                App.pnProyectosTipos.hide();
                App.btnNext.show();

                App.btnPrev.setDisabled(false);
                App.btnNext.setDisabled(true);

                
                App.btnAnadirProyecto.hide();
                App.winGestion.setTitle(jsFase);

                document.getElementById("lnkProyectos").lastChild.classList.remove("navActivo");
                document.getElementById("lnkFases").lastChild.classList.add("navActivo");
                document.getElementById("lnkZonas").lastChild.classList.remove("navActivo");
                document.getElementById("lnkEmpresaProveedora").lastChild.classList.remove("navActivo");
                document.getElementById("lnkProyectosTipos").lastChild.classList.remove("navActivo");
                tabActual = 4;

                break;
            case 3:
                App.pnFormProject.hide();
                App.pnPrincipalFases.hide();
                App.pnPrincipalZonas.show();
                App.pnPrincipalEmpresaProveedora.hide();
                App.pnProyectosTipos.hide();
                App.btnNext.show();

                App.btnPrev.setDisabled(false);
                App.btnNext.setDisabled(false);

                recargarCombos([App.cmbGlobalZonasLibres]);
                App.btnAnadirProyecto.hide();
                App.winGestion.setTitle(jsZona);

                document.getElementById("lnkProyectos").lastChild.classList.remove("navActivo");
                document.getElementById("lnkFases").lastChild.classList.remove("navActivo");
                document.getElementById("lnkZonas").lastChild.classList.add("navActivo");
                document.getElementById("lnkEmpresaProveedora").lastChild.classList.remove("navActivo");
                document.getElementById("lnkProyectosTipos").lastChild.classList.remove("navActivo");
                tabActual = 3;
                break;
            case 2:
                App.pnFormProject.hide();
                App.pnPrincipalFases.hide();
                App.pnPrincipalZonas.hide();
                App.pnPrincipalEmpresaProveedora.show();
                App.pnProyectosTipos.hide();

                App.btnPrev.setDisabled(false);
                App.btnNext.setDisabled(false);
                App.btnAnadirProyecto.hide();
                App.btnNext.show();
                App.winGestion.setTitle(jsEmpresaProveedora);
                recargarCombos([App.cmbEmpresasProveedorasLibres])
                document.getElementById("lnkProyectos").lastChild.classList.remove("navActivo");
                document.getElementById("lnkFases").lastChild.classList.remove("navActivo");
                document.getElementById("lnkZonas").lastChild.classList.remove("navActivo");
                document.getElementById("lnkEmpresaProveedora").lastChild.classList.add("navActivo");
                document.getElementById("lnkProyectosTipos").lastChild.classList.remove("navActivo");
                tabActual = 2;
                break;
            case 1:
                App.pnFormProject.hide();
                App.pnPrincipalFases.hide();
                App.pnPrincipalZonas.hide();
                App.pnPrincipalEmpresaProveedora.hide();
                App.pnProyectosTipos.show();

                App.btnNext.setDisabled(false);
                App.btnPrev.setDisabled(false);
                App.btnAnadirProyecto.hide();
                App.btnNext.show();
                App.winGestion.setTitle(jsProyectosTipos);
                recargarCombos([App.cmbTipoProyecto]);
                document.getElementById("lnkProyectos").lastChild.classList.remove("navActivo");
                document.getElementById("lnkFases").lastChild.classList.remove("navActivo");
                document.getElementById("lnkZonas").lastChild.classList.remove("navActivo");
                document.getElementById("lnkEmpresaProveedora").lastChild.classList.remove("navActivo");
                document.getElementById("lnkProyectosTipos").lastChild.classList.add("navActivo");
                tabActual = 1;
                break;
            default:
                App.pnFormProject.show();
                App.pnPrincipalFases.hide();
                App.pnPrincipalZonas.hide();
                App.pnPrincipalEmpresaProveedora.hide();

                App.btnPrev.setDisabled(true);
        }
    }


}

function DeseleccionarGrilla() {
    App.RowSelectProyectoTipo.clearSelections();
    App.GridRowSelect.clearSelections();
    App.btnAnadirProyectosTipos.setDisabled(true);
    App.btnEliminarProyectosTipos.setDisabled(true);
    App.btnActivar.setDisabled(true);
    App.btnActivarProyecto.setDisabled(true);
    App.RowSelectGlobalZonas.clearSelections();
    App.btnAnadirGlobalZonas.setDisabled(true);
    App.btnAnadirFases.setDisabled(true);
    App.btnEliminarGlobalZonas.setDisabled(true);
    App.btnActivarGlobalZonas.setDisabled(true);
    App.btnAnadirEmpresaProveedora.setDisabled(true);

    App.btnEditar.setDisabled(true);
    App.btnEliminar.setDisabled(true);
    App.btnDuplicar.setDisabled(true);

    parent.App.lnkProyectosSLA.setDisabled(true);
    parent.App.lnkProyectosUsuarios.setDisabled(true);
    parent.App.lbltituloPrincipal.setText(jsProyecto);
}

function refrescar() {
    App.GridRowSelect.clearSelections();
    App.RowSelectGlobalZonas.clearSelections();
    App.RowSelectProyectoTipo.clearSelections();
    App.RowSelectionModelEmpresaProveedora.clearSelections();
    App.hdProyectoSeleccionado.setValue('');
    App.storeProyectos.reload();
    App.storeFases.reload();
    App.storeProyectosGlobalZona.reload();
    App.storeProyectosProyectosTipos.reload();
    App.storeProyectosEmpresaProveedora.reload();

}

function asignarRender(sender, registro, index) {
    var datos = index.data;
    var html = '';
    html += "<div class='d-flx'>";

    html += "<ul class='ulGrid'>";
    html += "<li><h5>" + datos.Proyecto + "</h5></li>";
    html += "<li><span>" + datos.Descripcion + "</span></li>";
    var agrupacion = '';
    if (datos.ProyectoAgrupacion != null) {
        agrupacion = datos.ProyectoAgrupacion;
    }
    html += "<li><span class='bold p-s'>" + jsGrupo + ": </span>" + agrupacion + "</li>";
    html += "<li><span class='code'>" + datos.Referencia + "</span></li>";
    html += "</ul>";
    html += "<ul class='ulGrid ulGridCol2'> ";

    if (datos.Cerrado) {
        html += "<li class='liIcons'><span title='Cerrado'><img id='imClsd" + datos.ProyectoID + "'  src='../../ima/ico-success-gr.svg'></img><span></li>";
    } else {
        html += "<li class='liIcons'><span title='Cerrado'><img id='imClsd" + datos.ProyectoID + "' src=''></img><span></li>";
    }


    html += "<li><div class='ctIcons'>";
    if (datos.Multiflujo) {
        html += "<span class='icoGridItem' title='Multiflujo'><img id='imMultiflow" + datos.ProyectoID + "' src='../../ima/ico-subprocess.svg'></img></span>";
    } else {
        html += "<span class='icoGridItem' title='Multiflujo'><img id='imMultiflow" + datos.ProyectoID + "' src=''></img></span>";
    }

    if (datos.Activo) {
        html += "<span class='icoGridItem' title='Activo'><img id='imActivo" + datos.ProyectoID + "' src='../../ima/ico-accept.svg'></img></span>";

    } else {
        html += "<span class='icoGridItem' title='Activo'><img id='imActivo" + datos.ProyectoID + "' src=''></img></span>";

    }
    html += "</div></li>"

    html += "<li>";

    var anchoBarra = renderProgBar(datos.FechaInicio, datos.FechaFin, datos.ProyectoID);

    if (datos.FechaFin == undefined || datos.FechaFin == null) {
        html += "<div class='progBar-wr' title='Progress'><div id='progBar" + datos.ProyectoID + "' style='width:" + anchoBarra + "%' class='progBar'></div></div> <span> " + datos.FechaInicio.getDate() + "/" + (datos.FechaInicio.getMonth()+1) + "/" + datos.FechaInicio.getFullYear() + "</span> · <span> </span>";

    } else {
        html += "<div class='progBar-wr' title='Progress'><div id='progBar" + datos.ProyectoID + "' style='width:" + anchoBarra + "%' class='progBar'></div></div> <span> " + datos.FechaInicio.getDate() + "/" + (datos.FechaInicio.getMonth()+1) + "/" + datos.FechaInicio.getFullYear() + "</span> · <span>" + datos.FechaFin.getDate() + "/" + (datos.FechaFin.getMonth()+1) + "/" + datos.FechaFin.getFullYear() + "</span>";

    }
    html += "</li>"

    html += "</ul>";

    html += "</div>";

    return html;
}

function renderProgBar(fechaInicio, fechaFin) {
    var inicio = Math.round(fechaInicio.getTime() / (1000 * 60 * 60 * 24));
    var ahora = new Date();
    ahora = Math.round(ahora / (1000 * 60 * 60 * 24));
    if (fechaFin == undefined) {
        return "0";
    } else {
        var fin = Math.round(fechaFin.getTime() / (1000 * 60 * 60 * 24))

        if (ahora >= fin) {
            return "100";
        } else {
            var transcurrido = ahora - inicio;
            var total = fin - inicio;
            var porcentaje = transcurrido * 100 / total;
            return porcentaje;
        }
    }
    

}

function Grid_RowSelect(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionado = datos;
        parent.App.lbltituloPrincipal.setText(jsProyecto+": "+seleccionado.Proyecto);
        App.hdProyectoSeleccionado.setValue(seleccionado.ProyectoID);
        parent.App.hdProyectoID.setValue(seleccionado.ProyectoID);
        App.storeFases.reload();
        App.storeProyectosProyectosTipos.reload();
        App.storeProyectosGlobalZona.reload();
        App.storeProyectosEmpresaProveedora.reload();

        App.btnEditar.setDisabled(false);
        App.btnEliminar.setDisabled(false);
        App.btnDuplicar.setDisabled(false);
        App.btnActivarProyecto.setDisabled(false);


        App.btnAnadirFases.setDisabled(false);
        App.btnEliminarFases.setDisabled(true);
        App.btnRefrescarFases.setDisabled(false);

        App.btnAnadirProyectosTipos.setDisabled(false);
        App.btnEliminarProyectosTipos.setDisabled(true);
        App.btnRefrescarProyectoTipo.setDisabled(false);

        App.btnAnadirGlobalZonas.setDisabled(false);
        App.btnEliminarGlobalZonas.setDisabled(true);
        App.btnActivarGlobalZonas.setDisabled(true);
        App.btnRefrescarGlobalZonas.setDisabled(true);
        App.btnRefrescarGlobalZonas.setDisabled(false);

        App.btnAnadirEmpresaProveedora.setDisabled(false);
        App.btnRefrescarEmpresaProveedora.setDisabled(false);
        parent.App.lnkProyectosSLA.setDisabled(false);
        parent.App.lnkProyectosUsuarios.setDisabled(false);

        let iframe = "";
        iframe = parent.$("iframe[name='ctMain3_IFrame']")[0];
        iframe.src = iframe.src;
    }
}

// #region AGREGAR/EDITAR/ELIMINAR Proyecto

function VaciarFormulario() {
    App.pnFormProject.getForm().reset();
    App.pnPrincipalFases.getForm().reset();
    App.pnPrincipalZonas.getForm().reset();
    App.pnPrincipalEmpresaProveedora.getForm().reset();
    App.pnProyectosTipos.getForm().reset();
    App.btnMultiproceso.setPressed(false);

    Ext.each(App.pnFormProject.body.query('*'), function (value) {
        Ext.each(value, function (item) {
            var c = Ext.getCmp(item.id);
            if (c != undefined && c.isFormField) {
                c.reset();

                if (c.triggerWrap != undefined) {
                    c.triggerWrap.removeCls("itemForm-novalid");
                }

                if (!c.allowBlank && c.xtype != "checkboxfield" && c.xtype != "hidden") {
                    c.addListener("change", anadirClsNoValido, false);
                    c.addListener("focusleave", anadirClsNoValido, false);

                    c.removeCls("ico-exclamacion-10px-red");
                    c.addCls("ico-exclamacion-10px-grey");
                }

                if (c.allowBlank && c.cls == 'txtContainerCategorias') {
                    App[getIdComponente(c) + "_lbNombreAtr"].removeCls("ico-exclamacion-10px-grey");
                }
            }
        });
    });
}

function AgregarEditar() {
    accion = 'agregar';
    App.hdProyectoSeleccionado.setValue('');
    App.storeFases.reload();
    App.storeProyectosProyectosTipos.reload();
    App.storeProyectosGlobalZona.reload();
    App.storeProyectosEmpresaProveedora.reload();
    App.pnVistasForm.show();
    App.cntBtnForm.show();
    DeseleccionarGrilla();
    VaciarFormulario();
    NavegacionTab(0);

    var combos = [App.cmbGrupo, App.cmbMoneda, App.cmbEstado, App.cmbTipoProyecto, App.cmbGlobalZonasLibres, App.cmbEmpresasProveedorasLibres];
    showLoadMask(App.vwResp, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                App.winGestion.setTitle(jsAgregar + ' ' + jsProyecto);
                App.lnkFases.setDisabled(true);
                App.lnkZonas.setDisabled(true);
                App.lnkEmpresaProveedora.setDisabled(true);
                App.lnkProyectosTipos.setDisabled(true);
                App.btnNext.setDisabled(true);
                App.winGestion.show();
                load.hide()
            }

        });
    })

}

function winGestionAgregarEditar() {
    if (App.pnFormProject.getForm().isValid()) {
        ajaxAgregarEditar();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditar() {

    var Agregar = false;

    if (accion == 'agregar') {
        Agregar = true;
    }

    if (accion == 'duplicar') {
        TreeCore.Duplicar(
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        accion = 'editar';
                        App.lnkFases.setDisabled(false);
                        App.lnkZonas.setDisabled(false);
                        App.lnkEmpresaProveedora.setDisabled(false);
                        App.lnkProyectosTipos.setDisabled(false);
                        App.btnNext.setDisabled(false);
                        forzarCargaBuscadorPredictivo = true;
                        NavegacionTab(1);
                        App.storeProyectos.reload();
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    } else {
        TreeCore.AgregarEditar(Agregar,
            {
                success: function (result) {
                    if (result.Result != null && result.Result != '') {
                        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                    }
                    else {
                        if (Agregar) {
                            App.lnkProyectos.setDisabled(false);
                            accion = 'editar';
                        }
                        App.lnkFases.setDisabled(false);
                        App.lnkZonas.setDisabled(false);
                        App.lnkEmpresaProveedora.setDisabled(false);
                        App.lnkProyectosTipos.setDisabled(false);
                        App.btnNext.setDisabled(false);
                        forzarCargaBuscadorPredictivo = true;
                        NavegacionTab(1);
                        App.storeProyectos.reload();
                    }
                },
                eventMask:
                {
                    showMask: true,
                    msg: jsMensajeProcesando
                }
            });
    }
}

function MostrarEditar() {
    accion = 'editar'
    App.pnVistasForm.show();
    App.cntBtnForm.show();
    NavegacionTab(0);
    VaciarFormulario();
    if (registroSeleccionado(App.grdProjects) && seleccionado != null) {
        ajaxEditar();
    }
}

function ajaxEditar() {

    VaciarFormulario();
    App.storeProyectosTiposLibres.reload();
    App.storeGlobalZonasLibres.reload();
    App.storeEmpresasProveedorasLibres.reload();
    var combos = [App.cmbGrupo, App.cmbMoneda, App.cmbEstado];
    showLoadMask(App.vwResp, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                TreeCore.MostrarEditar(
                    {
                        success: function (result) {
                            if (result.Result != null && result.Result != '') {
                                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                            } else {
                                App.winGestion.setTitle(jsEditar + ' ' + jsProyecto);
                                App.winGestion.show();
                            }
                            load.hide();
                        }

                    }
                );
            }

        });
    })



}

function Eliminar() {
    if (registroSeleccionado(App.grdProjects) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsProyecto,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminar,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminar(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.Eliminar({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.btnEditar.setDisabled(false);
                    forzarCargaBuscadorPredictivo = true;
                    App.storeProyectos.reload();
                    App.storeFases.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function FormularioValidoProyecto(valid) {

    if (valid) {
        App.btnAnadirProyecto.setDisabled(false);
    }
    else {
        App.btnAnadirProyecto.setDisabled(true);
    }
}

function Duplicar() {
    accion = 'duplicar';
    App.pnVistasForm.show();
    App.cntBtnForm.show();
    App.pnFormProject.reset();
    NavegacionTab(0);
    VaciarFormulario();
    var combos = [App.cmbGrupo, App.cmbMoneda, App.cmbEstado];
    showLoadMask(App.vwResp, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                App.winGestion.setTitle(jsDuplicar + ' ' + jsProyecto);
                App.winGestion.setTitle(jsAgregar + ' ' + jsProyecto);
                App.lnkFases.setDisabled(true);
                App.lnkZonas.setDisabled(true);
                App.lnkEmpresaProveedora.setDisabled(true);
                App.lnkProyectosTipos.setDisabled(true);
                App.btnNext.setDisabled(true);
                App.winGestion.show();
                load.hide()
            }

        });
    })
}



function Activar() {
    if (registroSeleccionado(App.grdProjects)) {
        ajaxActivar();
    }
}

function ajaxActivar() {

    TreeCore.Activar({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                App.storeProyectos.reload();
            }
        },
        eventMask:
        {
            showMask: true,
            msg: jsMensajeProcesando
        }
    });
}
// #endregion

// #region AGREGAR/EDITAR/ELIMINAR Fases



function DeseleccionarGrillaFases() {
    App.gridRowSelectFases.clearSelections();
    App.btnEliminarFases.setDisabled(true);
}


var seleccionadoFase;

function Grid_RowSelectFases(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadoFase = datos;
        App.hdProyectoFaseID.setValue(seleccionadoFase.ProyectoFaseID);
        App.btnEliminarFases.setDisabled(false);
        App.btnRefrescarFases.setDisabled(false);
    }
}



function refrescarFases() {
    App.storeFases.reload();
}

function VaciarFormularioFases() {
    App.pnPrincipalFases.getForm().reset();
}

function FormularioValidoFases(valid) {
    if (valid) {
        App.btnAddPhase.setDisabled(false);
    }
    else {
        App.btnAddPhase.setDisabled(true);
    }
}

function AgregarEditarFases() {
    App.pnVistasForm.hide();
    App.pnFormProject.hide();
    App.pnPrincipalFases.show();
    App.pnPrincipalZonas.hide();
    App.pnPrincipalEmpresaProveedora.hide();
    App.pnProyectosTipos.hide();
    App.cntBtnForm.hide();

    VaciarFormularioFases();
    App.winGestion.setTitle(jsAgregar + ' ' + jsFase);
    App.winGestion.show();

}

function winGestionFasesAgregarEditar() {
    if (App.pnPrincipalFases.getForm().isValid() && App.hdProyectoSeleccionado.value != '') {
        ajaxAgregarEditarFases();
    }
    else {
        Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: jsFormularioNoValido, buttons: Ext.Msg.OK });
    }
}

function ajaxAgregarEditarFases() {

    TreeCore.AgregarEditarFases(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.btnEliminarFases.setDisabled(true);
                    App.txtNombreFases.setValue("");
                    App.storeFases.reload();
                }
            },
            eventMask:
            {
                showMask: true,
                msg: jsMensajeProcesando
            }
        }
    );


}

function EliminarFases() {
    if (registroSeleccionado(App.grdPhases) && seleccionado != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsFase,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarFases,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarFases(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarFases({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.btnAnadirFases.setDisabled(false);
                    App.storeFases.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

function EliminarProyectosFases(sender, registro, index) {

    seleccionadoFase = index.data.ProyectoFaseID;
    App.hdProyectoFaseID.setValue(seleccionadoFase);
    if (seleccionadoFase != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsFase,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarFases,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

// #endregion

// #region AGREGAR / ELIMINAR Proyecto Tipo


function DeseleccionarGrillaProyectosTipos() {
    App.RowSelectProyectoTipo.clearSelections();
    App.btnEliminarProyectosTipos.setDisabled(true);
    App.btnActivar.setDisabled(true);
}

function RefrescarProyectosTiposAsignados() {
    App.storeProyectosProyectosTipos.reload();
}

function FormularioValidoProyectoTipos(valid) {
    if (valid) {
        App.btnAnadirProyectoTipo.setDisabled(false);
    }
    else {
        App.btnAnadirProyectoTipo.setDisabled(true);
    }
}

var seleccionadoProyectoTipo;

function Grid_RowSelectProyectoTipo(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadoProyectoTipo = datos;
        App.hdProyectoProyectoTipoID.setValue(seleccionadoProyectoTipo.ProyectoProyectoTipoID);
        App.btnAnadirProyectosTipos.setDisabled(false);
        App.btnRefrescarProyectoTipo.setDisabled(false);
        App.btnEliminarProyectosTipos.setDisabled(false);
        App.btnActivar.setDisabled(false);
    }
}

function AgregarEditarProyectosTipos() {
    App.pnVistasForm.hide();
    App.pnFormProject.hide();
    App.pnPrincipalFases.hide();
    App.pnPrincipalZonas.hide();
    App.pnPrincipalEmpresaProveedora.hide();
    App.pnProyectosTipos.show();
    App.cntBtnForm.hide();

    var combos = [App.cmbTipoProyecto];
    showLoadMask(App.vwResp, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                App.winGestion.setTitle(jsAgregar + ' ' + jsProyectosTipos);
                //App.btnGuardarProyectoTipo.setDisabled(true);
                App.winGestion.show();
                load.hide()
            }

        });
    })
}

function EliminarProyectoTipo() {
    if (registroSeleccionado(App.gridProyectosTipos) && seleccionadoProyectoTipo != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsProyectosTipos,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarProyectoTipo,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarProyectoTipo(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarProyectoTipo({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.btnAnadirProyectoTipo.setDisabled(false);
                    App.btnEliminarProyectosTipos.setDisabled(true);
                    App.storeProyectosProyectosTipos.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

// #region Activar/Desactivar

function ActivarProyectoTipo() {
    if (registroSeleccionado(App.gridProyectosTipos) && gridProyectosTipos != null) {
        ajaxActivarProyectoTipo();
    }
}

function ajaxActivarProyectoTipo() {

    TreeCore.ActivarProyectoTipo({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                App.storeProyectosProyectosTipos.reload();
            }
        },
        eventMask:
        {
            showMask: true,
            msg: jsMensajeProcesando
        }
    });
}


// #endregion

// #endregion

// #region WIN ProyectosTipos

function recargarcmbTipoProyecto() {
    var combos = [App.cmbTipoProyecto];
    showLoadMask(App.winGestion, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                load.hide()
            }

        });
    })
}

function BotonGuardarProyectosTipoLibres() {

    TreeCore.AgregarProyectosTipos(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.cmbTipoProyecto.clearValue();
                    App.storeProyectosProyectosTipos.reload();
                    App.storeProyectosTiposLibres.reload();
                }
            }
        });

}

function EliminarProyectoProyectoTipo(sender, registro, index) {

    seleccionadoProyectoTipo = index.data.ProyectoProyectoTipoID;
    App.hdProyectoProyectoTipoID.setValue(seleccionadoProyectoTipo);
    if (seleccionadoProyectoTipo != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar + ' ' + jsProyectosTipos,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarProyectoTipo,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

//#endregion

// #region AGREGAR / ELIMINAR Zonas

function DeseleccionarGrillaZonas() {
    App.RowSelectGlobalZonas.clearSelections();
    App.btnEliminarGlobalZonas.setDisabled(true);
    App.btnActivarGlobalZonas.setDisabled(true);
}

function FormularioValidoZonas(valid) {
    if (valid) {
        App.btnAnadirZona.setDisabled(false);
    }
    else {
        App.btnAnadirZona.setDisabled(true);
    }
}

function RefrescarGlobalZonas() {
    App.storeProyectosGlobalZona.reload();

}

var seleccionadoGlobalZona;

function Grid_RowSelectGlobalZonas(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadoGlobalZona = datos;
        App.hdProyectoGlobalZonasID.setValue(seleccionadoGlobalZona.ProyectoGlobalZonaID);
        App.btnEliminarGlobalZonas.setDisabled(false);
        App.btnActivarGlobalZonas.setDisabled(false);
        App.btnRefrescarGlobalZonas.setDisabled(false);
    }
}

function AgregarEditarZonas() {
    App.pnVistasForm.hide();
    App.pnFormProject.hide();
    App.pnPrincipalFases.hide();
    App.pnPrincipalZonas.show();
    App.pnPrincipalEmpresaProveedora.hide();
    App.pnProyectosTipos.hide();
    App.cntBtnForm.hide();

    var combos = [App.cmbGlobalZonasLibres];
    showLoadMask(App.vwResp, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                App.winGestion.setTitle(jsAgregar + ' ' + jsZona);
                App.winGestion.show();
                load.hide()
            }

        });
    })
}

function EliminarGlobalZonas() {
    if (registroSeleccionado(App.grdZonas) && seleccionadoGlobalZona != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarGlobalZonas,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarGlobalZonas(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarGlobalZonas({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.btnAnadirGlobalZonas.setDisabled(false);
                    App.btnEliminarGlobalZonas.setDisabled(true);
                    App.storeProyectosGlobalZona.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

// #region Activar/Desactivar

function ActivarGlobalZonas() {
    if (registroSeleccionado(App.grdZonas) && seleccionadoGlobalZona != null) {
        ajaxActivarGlobalZonas();
    }
}

function ajaxActivarGlobalZonas() {

    TreeCore.ActivarGlobalZonas({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                App.storeProyectosGlobalZona.reload();
            }
        },
        eventMask:
        {
            showMask: true,
            msg: jsMensajeProcesando
        }
    });
}

// #endregion

// #endregion

// #region WIN Zonas

function recargarcmbGlobalZonasLibres() {
    var combos = [App.cmbGlobalZonasLibres];
    showLoadMask(App.winGestion, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                load.hide()
            }

        });
    })
}

function BotonGuardarGlobalZonasLibres() {

    TreeCore.AgregarGlobalZonas(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.cmbGlobalZonasLibres.clearValue();
                    App.storeProyectosGlobalZona.reload();
                    App.storeGlobalZonasLibres.reload();
                }
            }
        });
}

function EliminarProyectosGlobalZonas(sender, registro, index) {

    seleccionadoGlobalZona = index.data.ProyectoGlobalZonaID;
    App.hdProyectoGlobalZonasID.setValue(seleccionadoGlobalZona);
    if (seleccionadoGlobalZona != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarGlobalZonas,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

//#endregion

// #region AGREGAR / ELIMINAR EmpresasProveedoras

function DeseleccionarEmpresaProveedoras() {
    App.RowSelectionModelEmpresaProveedora.clearSelections();
    App.btnEliminarEmpresaProveedora.setDisabled(true);
    App.btnActivarEmpresaProveedora.setDisabled(true);
}

function FormularioValidoEmpresaProveedora(valid) {
    if (valid) {
        App.btnAnadirEmpresaProveedoras.setDisabled(false);
    }
    else {
        App.btnAnadirEmpresaProveedoras.setDisabled(true);
    }
}

function RefrescarEmpresasProveedoras() {
    App.storeProyectosEmpresaProveedora.reload();

}

var seleccionadoEmpresaProveedora;

function Grid_RowSelectEmpresaProveedora(sender, registro, index) {
    var datos = registro.data;

    if (datos != null) {
        seleccionadoEmpresaProveedora = datos;
        App.hdProyectoEmpresasProveedorasID.setValue(seleccionadoEmpresaProveedora.ProyectoEmpresaProveedoraID);
        App.btnEliminarEmpresaProveedora.setDisabled(false);
        App.btnActivarEmpresaProveedora.setDisabled(false);
        App.btnRefrescarEmpresaProveedora.setDisabled(false);
    }
}

function AgregarEditarEmpresaProveedora() {
    App.pnVistasForm.hide();
    App.pnFormProject.hide();
    App.pnPrincipalFases.hide();
    App.pnPrincipalZonas.hide();
    App.pnPrincipalEmpresaProveedora.show();
    App.pnProyectosTipos.hide();
    App.cntBtnForm.hide();

    var combos = [App.cmbEmpresasProveedorasLibres];
    showLoadMask(App.vwResp, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                App.winGestion.setTitle(jsAgregar + ' ' + jsEmpresaProveedora);
                App.winGestion.show();
                load.hide()
            }

        });
    })
}

function EliminarEmpresaProveedora() {
    if (registroSeleccionado(App.gridEmpresaProveedora) && seleccionadoEmpresaProveedora != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarEmpresaProveedora,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

function ajaxEliminarEmpresaProveedora(button) {
    if (button == 'yes' || button == 'si') {
        TreeCore.EliminarEmpresaProveedora({
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.btnAnadirEmpresaProveedora.setDisabled(false);
                    App.btnEliminarEmpresaProveedora.setDisabled(true);
                    App.btnActivarEmpresaProveedora.setDisabled(true);
                    App.storeProyectosEmpresaProveedora.reload();
                }
            },
            eventMask: { showMask: true, msg: jsMensajeProcesando }
        });
    }
}

// #region Activar/Desactivar

function ActivarEmpresaProveedora() {
    if (registroSeleccionado(App.gridEmpresaProveedora) && seleccionadoEmpresaProveedora != null) {
        ajaxActivarEmpresaProveedora();
    }
}

function ajaxActivarEmpresaProveedora() {

    TreeCore.ActivarEmpresaProveedora({
        success: function (result) {
            if (result.Result != null && result.Result != '') {
                Ext.Msg.alert({ title: jsAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
            }
            else {
                App.storeProyectosEmpresaProveedora.reload();
            }
        },
        eventMask:
        {
            showMask: true,
            msg: jsMensajeProcesando
        }
    });
}

// #endregion

// #endregion

// #region WIN EmpresaProveedora

function recargarcmbEmpresasProveedorasLibres() {
    var combos = [App.cmbEmpresasProveedorasLibres];
    showLoadMask(App.winGestion, function (load) {
        recargarCombos(combos, function (fin) {
            if (fin) {
                load.hide()
            }

        });
    })
}

function BotonGuardarEmpresaProveedoraLibres() {

    TreeCore.AgregarEmpresasProveedoras(
        {
            success: function (result) {
                if (result.Result != null && result.Result != '') {
                    Ext.Msg.show({ title: jsTituloAtencion, icon: Ext.MessageBox.WARNING, msg: result.Result, buttons: Ext.Msg.OK });
                }
                else {
                    App.cmbEmpresasProveedorasLibres.clearValue();
                    App.storeProyectosEmpresaProveedora.reload();
                    App.storeEmpresasProveedorasLibres.reload();
                }
            }
        });
}

function EliminarProyectosEmpresaProveedora(sender, registro, index) {

    seleccionadoEmpresaProveedora = index.data.ProyectoEmpresaProveedoraID;
    App.hdProyectoEmpresasProveedorasID.setValue(seleccionadoEmpresaProveedora);
    if (seleccionadoEmpresaProveedora != null) {
        Ext.Msg.alert(
            {
                title: jsEliminar,
                msg: jsMensajeEliminar,
                buttons: Ext.Msg.YESNO,
                fn: ajaxEliminarEmpresaProveedora,
                icon: Ext.MessageBox.QUESTION
            });
    }
}

//#endregion

