// live-search.js
$(document).ready(function () {
    // Attach an event listener to the input field for live search
    $('#searchInput').on('input', function () {
        // Get the search query from the input field
        var query = $(this).val().trim().toLowerCase();

        // Hide all rows
        $('tbody tr').hide();

        // Show rows that match the search query
        $('tbody tr').each(function () {
            var row = $(this);
            var rowData = row.text().toLowerCase();
            if (rowData.indexOf(query) !== -1) {
                row.show();
            }
        });
    });
});
