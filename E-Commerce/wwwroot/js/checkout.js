let selectedAddressId = null;
let selectedPaymentMethod = null;

function selectAddress(addressId) {
    selectedAddressId = addressId;
    document.getElementById('selectedAddressId').value = addressId;

    document.querySelectorAll('.address-card').forEach(card => {
        card.classList.remove('border-primary');
        if (parseInt(card.dataset.addressId) === addressId) {
            card.classList.add('border-primary');
        }
    });

    updateCheckoutButton();
}

function selectPaymentMethod(method) {
    selectedPaymentMethod = method;
    document.getElementById('selectedPaymentMethod').value = method;

    updateCheckoutButton();
}

function updateCheckoutButton() {
    const checkoutBtn = document.getElementById('checkoutBtn');
    if (selectedAddressId && selectedPaymentMethod) {
        checkoutBtn.disabled = false;
        checkoutBtn.innerHTML = '<i class="fas fa-lock me-2"></i>Complete Order';
    } else {
        checkoutBtn.disabled = true;
        checkoutBtn.innerHTML = '<i class="fas fa-lock me-2"></i>Select Address & Payment';
    }
}

function initializeCheckoutPage(addressId, paymentMethod) {
    if (addressId && addressId > 0) {
        selectAddress(addressId);
    }

    if (paymentMethod) {
        selectPaymentMethod(paymentMethod);
    }
}

document.addEventListener('DOMContentLoaded', function () {
    const shippingAddressId = document.getElementById('selectedAddressId')?.value;
    const paymentMethod = document.getElementById('selectedPaymentMethod')?.value;

    initializeCheckoutPage(shippingAddressId, paymentMethod);
});