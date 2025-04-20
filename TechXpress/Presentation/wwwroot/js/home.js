document.addEventListener('DOMContentLoaded', async () => {
    await updateCartCount();
    loadFeaturedProducts();
});

async function loadFeaturedProducts() {
    const productsGrid = document.getElementById('featured-products');
    if (productsGrid) {
        try {
            const response = await fetch('/Product/GetFeaturedProducts');
            if (response.ok) {
                const featuredProducts = await response.json();
                productsGrid.innerHTML = featuredProducts
                    .map(product => createProductCard(product))
                    .join('');
            }
        } catch (error) {
            console.error('Error loading featured products:', error);
        }
    }
}

async function updateCartCount() {
    try {
        const response = await fetch('/ShoppingCart/GetCartCount');
        if (!response.ok) {
            console.error('Failed to fetch cart count:', response.statusText);
            return;
        }
        const data = await response.json();
        const cartCountElement = document.getElementById('cartCount');
        if (cartCountElement) {
            cartCountElement.textContent = data.count || 0;
        }
    } catch (error) {
        console.error('Error updating cart count:', error);
    }
}

async function addToCart(productId, quantity) {
    try {
        const response = await fetch('/ShoppingCart/AddToCart', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ id: productId, quantity: quantity })
        });
        
        const result = await response.json();
        if (result.success) {
            await updateCartCount();
        } else {
            alert(result.message || 'Error adding to cart');
        }
    } catch (error) {
        console.error('Error adding to cart:', error);
    }
}

function createProductCard(product) {
    return `
        <div class="product-card">
            <img src="${product.imageUrl || '/images/default-product.jpg'}" alt="${product.name}">
            <h3>${product.name}</h3>
            <p class="price">$${product.price.toFixed(2)}</p>
            <button onclick="addToCart(${product.id}, 1)" class="add-to-cart-btn">Add to Cart</button>
        </div>
    `;
}
