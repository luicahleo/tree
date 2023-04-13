var calendar;
var tooltip;
var currentView = 'dayGridMonth';
const fecha = new Date().toISOString().split('T');
var dateToday = fecha[0];
var panelOculto = true;

function CalendarRender() {
    var calendarEl = document.getElementById('calendar');

    calendar = new FullCalendar.Calendar(calendarEl, {
        height: '100%',
        expandRows: true,
        //slotMinTime: '08:00',
        //slotMaxTime: '20:00',
        initialView: 'dayGridMonth',
        initialDate: dateToday,
        firstDay: 1,
        headerToolbar: false,
        editable: true,
        navLinks: true,
        navLinkDayClick: function (date, jsEvent) {
            navLinkCalendar(date);
        },
        selectable: true,
        dateClick: function (info) {
            closeWinCalendar();
        },
        nowIndicator: true,
        businessHours: true,
        fixedWeekCount: false,
        eventClick: function (info) {
            windowDescription(info);
        },
        //eventDidMount: function (info) {
        //    var tooltip = new bootstrap.Tooltip(info.el, {
        //        title: info.event.extendedProps.description,
        //        placement: 'top',
        //        trigger: 'hover',
        //        container: 'body'
        //    });
        //},
        windowResize: function (arg) {
            resizeCalendarWO();
        },
        dayMaxEventRows: true, // for all non-TimeGrid views
        dayMaxEvents: true, // allow "more" link when too many events
        eventColor: '#1c5471',
        events: [
            {
                id: '3833838-3',
                title: 'Action Name',
                start: '2021-11-01',
                color: '#1B4734',
                status: 'Statusname',
                workflow: 'WO Install',
                object: 'TR000-Site',
                service: 'Installing Tower',
                entity: 'Telcocom',
                durationText: 'SLA deviation: 2 days',
                category: 1,
                percentage: 65
            },
            {
                id: 002,
                title: 'Long Event',
                start: '2021-11-07',
                end: '2021-11-10',
                color: '#F7BB74',
                status: 'Statusname',
                workflow: 'WO Install',
                object: 'TR000-Site',
                service: 'Installing Tower',
                entity: 'Telcocom',
                durationText: 'SLA deviation: 2 days',
                category: 1,
                percentage: 80
            },
            {
                id: 003,
                groupId: 999,
                title: 'Repeating Event',
                start: '2021-11-11T16:00:00',
                status: 'Statusname',
                workflow: 'WO Install',
                object: 'TR000-Site',
                service: 'Installing Tower',
                entity: 'Telcocom',
                durationText: 'SLA deviation: 2 days',
                category: 2,
                percentage: 100
            },
            {
                id: 004,
                groupId: 999,
                title: 'Repeating Event',
                start: '2021-11-18T16:00:00',
                status: 'Statusname',
                workflow: 'WO Install',
                object: 'TR000-Site',
                service: 'Installing Tower',
                entity: 'Telcocom',
                durationText: 'SLA deviation: 2 days',
                category: 2,
                percentage: 0
            },
            {
                id: 005,
                groupId: 999,
                title: 'Repeating Event',
                start: '2021-11-25T16:00:00',
                status: 'Statusname',
                workflow: 'WO Install',
                object: 'TR000-Site',
                service: 'Installing Tower',
                entity: 'Telcocom',
                durationText: 'SLA deviation: 2 days',
                category: 2,
                percentage: 10
            },
            {
                id: 006,
                title: 'Conference',
                start: '2021-11-11',
                end: '2021-11-13',
                status: 'Statusname',
                workflow: 'WO Install',
                object: 'TR000-Site',
                service: 'Installing Tower',
                entity: 'Telcocom',
                durationText: 'SLA deviation: 2 days',
                percentage: 50
            },
            {
                id: 007,
                title: 'Meeting',
                start: '2021-11-12T10:30:00',
                end: '2021-11-12T12:30:00',
                status: 'Statusname',
                workflow: 'WO Install',
                object: 'TR000-Site',
                service: 'Installing Tower',
                entity: 'Telcocom',
                durationText: 'SLA deviation: 2 days',
                percentage: 65
            },
            {
                id: 0010,
                title: 'Lunch',
                start: '2021-11-12T12:00:00',
                status: 'Statusname',
                workflow: 'WO Install',
                object: 'TR000-Site',
                service: 'Installing Tower',
                entity: 'Telcocom',
                durationText: 'SLA deviation: 2 days',
                percentage: 95
            },
            {
                id: 0011,
                title: 'Meeting',
                start: '2021-11-12T14:30:00',
                status: 'Statusname',
                workflow: 'WO Install',
                object: 'TR000-Site',
                service: 'Installing Tower',
                entity: 'Telcocom',
                durationText: 'SLA deviation: 2 days',
                percentage: 75
            },
            {
                id: 0012,
                title: 'Happy Hour',
                start: '2021-11-12T17:30:00',
                status: 'Statusname',
                workflow: 'WO Install',
                object: 'TR000-Site',
                service: 'Installing Tower',
                entity: 'Telcocom',
                durationText: 'SLA deviation: 2 days',
                percentage: 25
            },
            {
                id: 0013,
                title: 'Dinner',
                start: '2021-11-12T20:00:00',
                status: 'Statusname',
                workflow: 'WO Install',
                object: 'TR000-Site',
                service: 'Installing Tower',
                entity: 'Telcocom',
                durationText: 'SLA deviation: 2 days',
                percentage: 95
            },
            {
                id: 0014,
                title: 'Birthday Party',
                start: '2021-11-13T07:00:00',
                status: 'Statusname',
                workflow: 'WO Install',
                object: 'TR000-Site',
                service: 'Installing Tower',
                entity: 'Telcocom',
                durationText: 'SLA deviation: 2 days',
                percentage: 36
            },
            {
                id: 0015,
                title: 'Click for Google',
                url: '',
                start: '2021-11-28',
                color: '#EF5970',
                status: 'Statusname',
                workflow: 'WO Install',
                object: 'TR000-Site',
                service: 'Installing Tower',
                entity: 'Telcocom',
                durationText: 'SLA deviation: 2 days',
                category: 3,
                percentage: 100
            }
        ]
    });

    calendar.render();
}

function selectView(sender) {
    var view = sender.value;
    closeWinCalendar();

    if (view == 'day') {
        calendar.changeView('timeGridDay');
        currentView = 'timeGridDay';
        labelCalendar('timeGridDay');
    }
    else if (view == 'week') {
        calendar.changeView('timeGridWeek');
        currentView = 'timeGridWeek';
        labelCalendar('timeGridWeek');
    }
    else if (view == 'month') {
        calendar.changeView('dayGridMonth');
        currentView = 'dayGridMonth';
        labelCalendar('dayGridMonth');
    }
    else if (view == 'list') {
        calendar.changeView('listYear');
        currentView = 'listYear';
        labelCalendar('listYear');
    }
}

function viewToday() {
    closeWinCalendar();
    calendar.today();
    labelCalendar(calendar.view.type);
}

function changePrevOrNext(prevOrNext) {
    if (prevOrNext == 'Prev') {
        calendar.prev();
    }
    else if (prevOrNext == 'Next') {
        calendar.next();
    }
    closeWinCalendar();
    labelCalendar(calendar.view.type);
}

function labelCalendar(view) {
    App.lbMonth.setText(calendar.view.title);

    if (view == 'timeGridDay') {
        App.lbMonth.setWidth(200);
    }
    else if (view == 'timeGridWeek') {
        App.lbMonth.setWidth(250);
    }
    else if (view == 'dayGridMonth') {
        App.lbMonth.setWidth(150);
    }
    else if (view == 'listYear') {
        App.lbMonth.setWidth(85);
    }

}

function resizeCalendarWO() {
    let puntoCorteS = 500;
    let puntoCorteL = 730;
    var tmn = App.pnCol1.getWidth();
    closeWinCalendar();

    if (tmn < puntoCorteS) {
        App.pnCategoryCalendar.hide();
        App.btnPanelCategory.show();
        if (currentView != 'listYear') {
            calendar.changeView('timeGridDay');
        }
        App.cmbMonth.disable();
        calendar.updateSize();
    }
    else if (tmn > puntoCorteS && tmn < puntoCorteL) {
        if (currentView != 'listYear') {
            calendar.changeView('timeGridDay');
        }
        App.pnCategoryCalendar.show();
        App.btnPanelCategory.hide();

        App.cmbMonth.disable();
    }
    else {
        App.pnCategoryCalendar.show();
        App.btnPanelCategory.hide();
        calendar.changeView(currentView);
        calendar.updateSize();

        App.cmbMonth.enable();
    }

    labelCalendar(calendar.view.type);
}

function navLinkCalendar(date) {
    closeWinCalendar();
    calendar.changeView('timeGridDay');
    App.cmbMonth.setValue('day');
    calendar.gotoDate(new Date(date));
    labelCalendar(calendar.view.type);
}

function windowDescription(info) {
    document.getElementById('modalID').innerHTML = info.event.id;
    document.getElementById('modalTitle').innerHTML = info.event.title;
    document.getElementById('modalWorkFlow').innerHTML = info.event.extendedProps.workflow;
    document.getElementById('modalStatus').innerHTML = info.event.extendedProps.status;
    document.getElementById('modalObject').innerHTML = info.event.extendedProps.object;
    document.getElementById('modalService').innerHTML = info.event.extendedProps.service;
    document.getElementById('modalEntity').innerHTML = info.event.extendedProps.entity;
    document.getElementById('modalSLA').innerHTML = info.event.extendedProps.durationText;
    App.pbCalendarDescription.setValue(info.event.extendedProps.percentage / 100);
    renderProgressBarCalendario(info.event.extendedProps.percentage / 100);
    document.getElementById('modalPercentage').innerHTML = info.event.extendedProps.percentage + "%";

    //if (info.jsEvent.pageX > calendar.el.offsetWidth && info.jsEvent.pageY + document.getElementById('fullCalModal').offsetHeight > calendar.el.offsetHeight) {
    //    document.getElementById('fullCalModal').style = "left: " + (info.jsEvent.pageX - 400) + "px; top: " + ((info.jsEvent.pageY - 90) - document.getElementById('fullCalModal').offsetHeight) + "px;";
    //}
    //else if (info.jsEvent.pageX > calendar.el.offsetWidth) {
    //    document.getElementById('fullCalModal').style = "left: " + (info.jsEvent.pageX - 400) + "px; top: " + (info.jsEvent.pageY - 90) + "px;";
    //}
    //else if (info.jsEvent.pageY + document.getElementById('fullCalModal').offsetHeight > calendar.el.offsetHeight) {
    //    document.getElementById('fullCalModal').style = "left: " + (info.jsEvent.pageX - 200) + "px; top: " + ((info.jsEvent.pageY - 90) - document.getElementById('fullCalModal').offsetHeight) + "px;";
    //}
    //else {
    //    document.getElementById('fullCalModal').style = "left: " + (info.jsEvent.pageX - 200) + "px; top: " + (info.jsEvent.pageY - 90) + "px;";
    //}

    if (info.jsEvent.pageX > calendar.el.offsetWidth && info.jsEvent.pageY + document.getElementById('fullCalModal').offsetHeight > calendar.el.offsetHeight) {
        document.getElementById('fullCalModal').style = "left: " + (calendar.el.offsetWidth - 200) + "px; top: " + (calendar.el.offsetHeight - document.getElementById('fullCalModal').offsetHeight) + "px;";
    }
    else if (info.jsEvent.pageX > calendar.el.offsetWidth) {
        document.getElementById('fullCalModal').style = "left: " + (calendar.el.offsetWidth - 200) + "px; top: " + (info.jsEvent.pageY - 90) + "px;";
    }
    else if (info.jsEvent.pageY + document.getElementById('fullCalModal').offsetHeight > calendar.el.offsetHeight) {
        document.getElementById('fullCalModal').style = "left: " + (info.jsEvent.pageX - 200) + "px; top: " + (calendar.el.offsetHeight - document.getElementById('fullCalModal').offsetHeight) + "px;";
    }
    else {
        document.getElementById('fullCalModal').style = "left: " + (info.jsEvent.pageX - 200) + "px; top: " + (info.jsEvent.pageY - 90) + "px;";
    }

    document.getElementById('fullCalModal').style.display = 'block'; //changed just for demo purposes
}

function closeWinCalendar() {
    document.getElementById('fullCalModal').style.display = 'none'; //changed just for demo purposes
}

function selectCategory(sender, category) {
    var listEvent = calendar.getEvents();
    closeWinCalendar();

    listEvent.forEach(function (itm) {
        if (itm.extendedProps.category == category) {
            if (sender.checked) {
            }
            else {
                //itm.display = 'none';
                //calendar.refetchEvents();
                itm.remove();
            }
        }
    });
}

function seePanelCategory() {
    let btn = document.getElementById('btnPanelCategory');

    if (panelOculto) {
        panelOculto = false;
        App.pnCategoryCalendar.show();
        btn.style.transform = 'rotate(-180deg)';
    }
    else {
        panelOculto = true;
        App.pnCategoryCalendar.hide();
        btn.style.transform = 'rotate(0deg)';
    }
}

function renderProgressBarCalendario(value) {
    let progressBar = App.pbCalendarDescription;

    if (value <= "0.15") {
        progressBar.addCls('progressBarRed');
        progressBar.removeCls('progressBarYellow');
    }
    else if (value > "0.15" && value <= "0.80") {
        progressBar.addCls('progressBarYellow');
        progressBar.removeCls('progressBarRed');
    }
    else if (value > "0.80" && value <= "1") {
        progressBar.removeCls('progressBarRed');
        progressBar.removeCls('progressBarYellow');
    }
}

//#region DIRECT METHODS

function Refrescar() {
    closeWinCalendar();
    CalendarRender();
}

//#endregion