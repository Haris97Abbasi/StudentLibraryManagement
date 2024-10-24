document.addEventListener('DOMContentLoaded', function () {
    const roleSelect = document.getElementById('roleSelect');

    // Refresh the page when the role is changed
    roleSelect.addEventListener('change', function () {
        window.location.href = window.location.pathname + '?RoleName=' + roleSelect.value;
    });

    // Remove validation from fields when Admin is selected
    const form = $("form");
    if (roleSelect.value === "Admin") {
        form.find('input[name="Gender"]').rules('remove');
        form.find('input[name="Age"]').rules('remove');
        form.find('input[name="Department"]').rules('remove');
    }
});
