document.addEventListener('DOMContentLoaded', () => {
    const productsGrid = document.getElementById('products-grid');
    const categoryInputs = document.querySelectorAll('input[name="category"]');
    const minPriceInput = document.getElementById('min-price');
    const maxPriceInput = document.getElementById('max-price');
    const applyPriceBtn = document.getElementById('apply-price');
    const clearFiltersBtn = document.getElementById('clear-filters');
    const sortSelect = document.getElementById('sort-select');
    const gridViewBtn = document.querySelector('.grid-view');
    const listViewBtn = document.querySelector('.list-view');
    const noResultsDiv = document.querySelector(".no-results");

    let currentFilters = {
        category: '',
        minPrice: '',
        maxPrice: '',
        sortBy: 'default',
        page: 1
    };

    function filterProducts() {
        const categoryId = Array.from(categoryInputs).find(radio => radio.checked)?.value || "";
        const minPrice = minPriceInput.value || "";
        const maxPrice = maxPriceInput.value || "";
        const sortBy = sortSelect.value;
        const page = currentFilters.page;

        const query = new URLSearchParams({
            categoryId: categoryId,
            minPrice: minPrice,
            maxPrice: maxPrice,
            sortBy: sortBy,
            page: page
        }).toString();

        fetch(`/Product/Index?${query}`)
            .then(response => response.text())
            .then(html => {
                const parser = new DOMParser();
                const doc = parser.parseFromString(html, "text/html");
                const newProductsGrid = doc.getElementById("products-grid");
                const newNoResults = doc.querySelector(".no-results");
                const newPagination = doc.querySelector(".pagination");

                if (newProductsGrid) {
                    productsGrid.innerHTML = newProductsGrid.innerHTML;
                    productsGrid.style.display = "grid";
                    noResultsDiv.style.display = "none";
                } else if (newNoResults) {
                    productsGrid.style.display = "none";
                    noResultsDiv.style.display = "block";
                    noResultsDiv.innerHTML = newNoResults.innerHTML;
                }
                if (newPagination) {
                    document.querySelector(".pagination").innerHTML = newPagination.innerHTML;
                }
            })
            .catch(error => console.error("Error applying filters:", error));
    }


    categoryInputs.forEach(input => {
        input.addEventListener('change', (e) => {
            currentFilters.category = e.target.value;
            currentFilters.page = 1;
            filterProducts();
        });
    });

    applyPriceBtn.addEventListener('click', () => {
        currentFilters.minPrice = minPriceInput.value ? parseFloat(minPriceInput.value) : '';
        currentFilters.maxPrice = maxPriceInput.value ? parseFloat(maxPriceInput.value) : '';
        currentFilters.page = 1; 
        filterProducts();
    });

    clearFiltersBtn.addEventListener('click', () => {
        currentFilters = {
            category: '',
            minPrice: '',
            maxPrice: '',
            sortBy: 'default',
            page: 1
        };

        categoryInputs.forEach(radio => (radio.checked = false));
        minPriceInput.value = '';
        maxPriceInput.value = '';
        sortSelect.value = 'default';
        filterProducts();
    });

    sortSelect.addEventListener('change', (e) => {
        currentFilters.sortBy = e.target.value;
        currentFilters.page = 1; 
        filterProducts();
    });

    gridViewBtn.addEventListener('click', () => {
        productsGrid.classList.remove('list-view');
        gridViewBtn.classList.add('active');
        listViewBtn.classList.remove('active');
    });

    listViewBtn.addEventListener('click', () => {
        productsGrid.classList.add('list-view');
        listViewBtn.classList.add('active');
        gridViewBtn.classList.remove('active');
    });

    document.addEventListener('click', (e) => {
        if (e.target.tagName === 'A' && e.target.getAttribute('asp-action') === 'Index') {
            e.preventDefault();
            const page = parseInt(e.target.textContent);
            currentFilters.page = page;
            filterProducts();
        }
    });

    window.addToCart = (productId) => {
        fetch(`/ShoppingCart/AddToCart?productId=${productId}`, {
            method: 'POST',
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    alert('Product added to cart!');
                    const cartCount = document.getElementById('cart-count');
                    if (cartCount) cartCount.textContent = data.cartCount;
                } else {
                    alert('Error adding to cart!');
                }
            })
            .catch(error => console.error('Error:', error));
    };


    filterProducts();
});