document.addEventListener('DOMContentLoaded', () => {
    // Initialize the cart count
    Helpers.updateCartCount();

    // Load and display featured products
    const featuredProducts = Product.getAll().slice(0, 3); // Show first 3 products as featured
    const productsGrid = document.getElementById('featured-products');
    
    if (productsGrid) {
        productsGrid.innerHTML = featuredProducts
            .map(product => Helpers.createProductCard(product))
            .join('');
    }
});
