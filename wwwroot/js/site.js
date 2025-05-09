document.addEventListener('DOMContentLoaded', function () {
    let selectedDate = null;
    let calendarEl = document.getElementById('calendar');

    let calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'timeGridWeek',
        footerToolbar: true,
        selectable: true,
        locale: 'it',
        nowIndicator: true,
        timeZone: 'local',
        firstDay: 1,
        slotMinTime: "05:00:00",
        slotMaxTime: "23:30:00",
        allDaySlot: false,
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'dayGridMonth,timeGridWeek,timeGridDay'
        },
        buttonText: {
            today: 'oggi',
            month: 'mese',
            week: 'settimana',
            day: 'giorno',
            list: 'lista'
        },
        events: '/Home/GetEvents',

        dateClick: async function (info) {

            selectedDate = new Date(info.dateStr);
            document.getElementById('eventDate').value = selectedDate.toISOString().split('T')[0];
            document.getElementById('eventTime').value = selectedDate.toTimeString().slice(0, 5);

            initializeModal('empty');

            getInfoAlert();

        },

        eventClick: function (info) {
            alert('Evento: ' + info.event.title);
            console.log(info)
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

        let colorEl = document.getElementById('eventTitle');
        let selectedOption = colorEl.options[colorEl.selectedIndex];
        let color = selectedOption.dataset.color;

        if (title && date && time && duration > 0) {
            let start = new Date(date + 'T' + time);
            let end = new Date(start.getTime() + duration * 60000);


            //se l'utente NON esiste
            if (customerId <= 0 || customerId == "") {

                let customerName = document.getElementById('customerName').value;
                let customerSurname = document.getElementById('customerSurname').value;
                let customerEmail = document.getElementById('customerEmail').value || "";
                let customerPhone = document.getElementById('customerPhone').value;
                
                const eventData = {
                    CustomerName: customerName,
                    CustomerSurname: customerSurname,
                    CustomerEmail: customerEmail,
                    CustomerPhone: customerPhone,
                    Title: title,
                    Start: start.toISOString(),
                    End: end.toISOString(),
                    Duration: duration,
                    AllDay: false,
                    Notes: notes,
                    Color: color
                };

                try {
                    const response = await fetch("/Home/CreateCustomerAndEvent", {
                        method: "POST",
                        headers: {
                            "Content-Type": "application/json"
                        },
                        body: JSON.stringify(eventData)
                    });
    
                    if (!response.ok) {
                        showSimpleAlert("error", "Errore di comunicazione.");
                        throw new Error(`Response status: ${response.status}`);
                    }
    
                    const json = await response.json();
                    const obj = JSON.parse(json);
                    if(!obj){
                        showSimpleAlert("error", "Errore nell'inserimento.");
                        return;
                    }
    
                    // Aggiungi l'evento nel calendario
                    calendar.addEvent({
                        title: title,
                        start: start,
                        end: end,
                        backgroundColor: color,
                        borderColor: color,
                    });
    
                    let modal = bootstrap.Modal.getInstance(document.getElementById('eventModal'));
                    modal.hide();
    
                    showSimpleAlert("success", "Evento inserito con successo!");
    
                } catch (error) {
                    console.error("Errore durante la creazione dell'evento:", error.message);
                }

            } else {

                const eventData = {
                    CustomerId: customerId,
                    Title: title,
                    Start: start.toISOString(),
                    End: end.toISOString(),
                    Duration: duration,
                    AllDay: false,
                    Notes: notes,
                    Color: color
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
                        showSimpleAlert("error", "Errore di comunicazione.");
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
                        end: end,
                        backgroundColor: color,
                        borderColor: color,
                    });
    
                    let modal = bootstrap.Modal.getInstance(document.getElementById('eventModal'));
                    modal.hide();
    
                    showSimpleAlert("success", "Evento inserito con successo!");
    
                    removeCustomerFromList(customerId);
    
                } catch (error) {
                    console.error("Errore durante la creazione dell'evento:", error.message);
                }
            }

        } else {
            showSimpleAlert("error", "Compila tutti i campi obbligatori");
        }
    });

    calendar.render();
});

//inizializzazione del modal
function initializeModal(modalType, element){
    if(modalType == 'empty'){

        document.getElementById('customerName').value = '';
        document.getElementById('customerSurname').value = '';
        document.getElementById('customerEmail').value = '';
        document.getElementById('customerPhone').value = '';
        document.getElementById('customerId').value = '';
        document.getElementById('eventDuration').value = 0;
        document.getElementById('eventNotes').value = '';
        document.getElementById('eventTitle').value = '';

    }else{

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
        document.getElementById('eventDate').value = "";
        document.getElementById('eventTime').value = "";
        document.getElementById('eventDuration').value = 0;
        document.getElementById('eventNotes').value = "";
        document.getElementById('eventTitle').value = "";

        let eventModal = new bootstrap.Modal(document.getElementById('eventModal'));
        eventModal.show();
    }

    return;
}

//Assegnazione automatica della durata tramite la select
document.getElementById('eventTitle').addEventListener('change', function () {
    const durationInput = document.getElementById('eventDuration');
    const selectedValue = this.value;

    switch (selectedValue) {
        case 'Igiene professionale':
            durationInput.value = 30; 
            break;
        case 'Igiene professionale + illuminante':
            durationInput.value = 40; 
            break;
        case 'Sbiancamento':
            durationInput.value = 30;
            break;
        default:
            durationInput.value = 0; 
            break;
    }
});

/*Rimuovi cliente dalla lista dopo aver preso appuntamento */
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

//Prendo la lista dei clienti
async function openSearchUserAlert() {
    await Swal.fire({
        title: 'Cerca cliente',
        html: `
            <input type="text" id="searchInput" class="form-control mb-3" placeholder="Scrivi nome o cognome...">
            <div id="searchResults" style="max-height: 200px; overflow-y: auto;"></div>
        `,
        showCancelButton: true,
        confirmButtonText: 'Seleziona',
        cancelButtonText: 'Annulla',
        preConfirm: () => {
            const customerSelect = document.getElementById('customerSelect');
            if (customerSelect && customerSelect.value) {
                return customerSelect.value;
            } else {
                Swal.showValidationMessage('Seleziona un cliente');
                return false;
            }
        },
        didOpen: () => {
            const searchInput = document.getElementById('searchInput');
            searchInput.focus();
            searchInput.addEventListener('input', async function () {
                const query = this.value.trim();
                if (query.length >= 2) {
                    const response = await fetch(`/Home/SearchCustomers?query=${encodeURIComponent(query)}`);
                    const customers = await response.json();
                    const resultsContainer = document.getElementById('searchResults');
        
                    if (customers.length === 0) {
                        resultsContainer.innerHTML = '<div class="text-muted">Nessun risultato</div>';
                    } else {
                        resultsContainer.innerHTML = `
                            <select class="form-select" size="5" id="customerSelect" required>
                                ${customers.map(c => `
                                    <option value="${c.id}">${c.name} ${c.surname}</option>
                                `).join('')}
                            </select>
                        `;
                    }
                } else {
                    document.getElementById('searchResults').innerHTML = '';
                }
            });
        }
    }).then((result) => {
        if (result.isConfirmed && result.value) {
            const selectedCustomerId = result.value;
            loadCustomerData(selectedCustomerId);
        }
    });
}

//carico i dati del cliente già esistente
async function loadCustomerData(customerId) {
    const response = await fetch(`/Home/GetCustomerById?id=${customerId}`);
    const customer = await response.json();

    document.getElementById('customerId').value = customer.id;
    document.getElementById('customerName').value = customer.name;
    document.getElementById('customerSurname').value = customer.surname;
    document.getElementById('customerEmail').value = customer.email || "";
    document.getElementById('customerPhone').value = customer.mobile;

    let eventModal = new bootstrap.Modal(document.getElementById('eventModal'));
    eventModal.show();
}


//prendo i dati e mostro alert cliccando sull'evento
let events = document.querySelectorAll(".fc-event");
console.log(events)

//TODO fare endpoint per prendere dati dell'evento quando ci clicco sopra 