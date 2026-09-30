// ===== Traversal Panel – gemeinsame Skripte =====
(function ($) {
    // Sicherheitsabfrage für Lösch-/Stornier-Formulare: <form data-confirm="Wirklich löschen?">
    $(document).on('submit', 'form[data-confirm]', function (e) {
        if (!window.confirm($(this).data('confirm'))) e.preventDefault();
    });

    // Bildvorschau: <input type="file" data-preview="#img">
    $(document).on('change', 'input[type=file][data-preview]', function () {
        var target = $($(this).data('preview'));
        if (this.files && this.files[0]) {
            var reader = new FileReader();
            reader.onload = function (ev) { target.attr('src', ev.target.result).removeClass('d-none'); };
            reader.readAsDataURL(this.files[0]);
        }
    });

    // Meldungen nach 6 Sekunden ausblenden
    setTimeout(function () { $('.alert-dismissible').alert('close'); }, 6000);

    // KI-Assistent
    $('#ai-form').on('submit', function (e) {
        e.preventDefault();
        var city = $('#ai-city').val().trim();
        if (!city) return;
        var box = $('#ai-messages');
        box.append($('<div class="ai-msg ai-msg-user"></div>').text(city));
        $('#ai-city').val('');
        var loading = $('<div class="ai-msg ai-msg-bot"><i class="fas fa-spinner fa-spin"></i> Einen Moment …</div>').appendTo(box);
        box.scrollTop(box[0].scrollHeight);
        $.ajax({
            url: '/AI/GetCityInfo', type: 'POST', contentType: 'application/json',
            headers: { 'RequestVerificationToken': $('#ai-form input[name=__RequestVerificationToken]').val() },
            data: JSON.stringify({ cityName: city })
        }).done(function (items) {
            loading.remove();
            (items || []).forEach(function (i) {
                var m = $('<div class="ai-msg ai-msg-bot"></div>');
                m.append($('<strong class="d-block"></strong>').text(i.title));
                m.append($('<span></span>').text(i.description));
                box.append(m);
            });
            box.scrollTop(box[0].scrollHeight);
        }).fail(function (xhr) {
            loading.remove();
            box.append($('<div class="ai-msg ai-msg-bot ai-msg-error"></div>').text(xhr.responseText || 'Fehler bei der Anfrage.'));
        });
    });
})(jQuery);
