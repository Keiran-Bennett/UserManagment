var logModal = document.getElementById('logModal');
logModal.addEventListener('show.bs.modal', function (event) {
    UpdateModal(event);
});

function UpdateModal(event) {
    UpdateLogDetails(event);
    UpdateUserDetails(event);
}

function UpdateLogDetails(event) {
    SetModalInfo('modalLogDate', GetButtonData(event, 'log-date'));
    SetModalInfo('modalLogType', GetButtonData(event, 'log-type'));
    SetModalInfo('modalLogDetails', GetButtonData(event, 'log-details'));
}

function UpdateUserDetails(event) {
    SetModalInfo('modalUserForename', GetButtonData(event, 'user-forename'));
    SetModalInfo('modalUserSurname', GetButtonData(event, 'user-surname'));
    SetModalInfo('modalUserEmail', GetButtonData(event, 'user-email'))
    SetModalInfo('modalUserDob', GetButtonData(event, 'user-dob'));

    var c = $(event.relatedTarget).data('user-active'); 
    var userActiveText = $(event.relatedTarget).data('user-active') === 'True' ? 'Yes' : 'No';
    SetModalInfo('modalUserActive', userActiveText);
}

function GetButtonData(event, attributeName) {
    var button = $(event.relatedTarget);
    return button.data(attributeName);
}

function SetModalInfo(id, content) {
    $("#" + id).text(content);
}
