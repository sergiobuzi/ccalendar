/*
*CALENDARIO*
*/
document.addEventListener('DOMContentLoaded', function () {
    let selectedDate = null;
    let calendarEl = document.getElementById('calendar');

    let calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'timeGridWeek',
        themeSystem: 'bootstrap5',
        selectable: true,
        locale: 'it',
        nowIndicator: true,
        timeZone: 'local',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        events: '/Home/GetEvents',

        dateClick: function (info) {
            selectedDate = new Date(info.dateStr);

            document.getElementById('eventTitle').value = '';
            document.getElementById('eventDate').value = selectedDate.toISOString().split('T')[0];
            document.getElementById('eventTime').value = selectedDate.toTimeString().slice(0, 5);
            document.getElementById('eventDuration').value = 60;

            let eventModal = new bootstrap.Modal(document.getElementById('eventModal'));
            eventModal.show();
        },

        eventClick: function (info) {
            alert('Evento: ' + info.event.title);
        },

    });

    //Salvataggio dell'evento dal modale
    document.getElementById('saveEventBtn').addEventListener('click', async function () {

        let customerId = document.getElementById('customerId').value;
        let title = document.getElementById('eventTitle').value;
        let date = document.getElementById('eventDate').value;
        let time = document.getElementById('eventTime').value;
        let duration = parseInt(document.getElementById('eventDuration').value);
        let notes = document.getElementById('eventNotes').value;

        if (title && date && time && duration > 0) {
            let start = new Date(date + 'T' + time);
            let end = new Date(start.getTime() + duration * 60000);

            const eventData = {
                CustomerId: customerId,
                Title: title,
                Start: start.toISOString(),
                End: end.toISOString(),
                Duration: duration,
                AllDay: false,
                Notes: notes
            };

            try {
                const response = await fetch("/Home/CreateEvent", {
                    method: "POST",
                    headers: {
                        "Content-Type": "application/json"
                    },
                    body: JSON.stringify(eventData)
                });

                if (!response.ok) {
                    throw new Error(`Response status: ${response.status}`);
                }

                const json = await response.json();
                const obj = JSON.parse(json);
                if(!obj){
                    showSimpleAlert("error", "Errore nell'inserimento dell'evento.");
                    return;
                }

                // Aggiungi l'evento nel calendario
                calendar.addEvent({
                    title: title,
                    start: start,
                    end: end
                });

                let modal = bootstrap.Modal.getInstance(document.getElementById('eventModal'));
                modal.hide();

                showSimpleAlert("success", "Evento inserito con successo!");

                removeCustomerFromList(customerId);

            } catch (error) {
                console.error("Errore durante la creazione dell'evento:", error.message);
            }
        } else {
            showSimpleAlert("error", "Compila tutti i campi obbligatori");
        }
    });

    calendar.render();
});

/*
*NAVBAR*
*/

async function openModalWithCustomerData(element) {
    const id = element.dataset.customerId;
    const name = element.dataset.name;
    const surname = element.dataset.surname;
    const mobile = element.dataset.mobile;
    const email = element.dataset.email;

    // Valorizza i campi del modale
    document.getElementById('customerName').value = name;
    document.getElementById('customerSurname').value = surname;
    document.getElementById('customerEmail').value = email;
    document.getElementById('customerPhone').value = mobile;
    document.getElementById('customerId').value = id;

    // Mostra il modale
    const myModal = new bootstrap.Modal(document.getElementById('eventModal'));
    myModal.show();
};

function removeCustomerFromList(customerId){
    const customerItems = document.querySelectorAll('.customer-element');

    customerItems.forEach(item => {
        if (item.dataset.customerId === customerId.toString()) {
            item.style.display = 'none';
        }
    });

    // Aggiorna il badge (se esiste)
    let badge = document.getElementById('customer-list-badge-number');
    if (badge) {
        let number = parseInt(badge.textContent);
        if (number > 0) {
            badge.textContent = number - 1;
        }
    }

}