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
    const paginationContainer = document.querySelector(".pagination");

    let currentFilters = {
        categoryId: '',
        minPrice: '',
        maxPrice: '',
        sortBy: 'default',
        page: 1
    };

    function buildQueryString() {
        return new URLSearchParams({
            categoryId: currentFilters.categoryId,
            minPrice: currentFilters.minPrice,
            maxPrice: currentFilters.maxPrice,
            sortBy: currentFilters.sortBy,
            page: currentFilters.page
        }).toString();
    }

    function filterProducts() {
        fetch(`/Product/Index?${buildQueryString()}`)
            .then(res => res.text())
            .then(html => {
                const parser = new DOMParser();
                const doc = parser.parseFromString(html, "text/html");

                const newProducts = doc.getElementById("products-grid");
                const newNoResults = doc.querySelector(".no-results");
                const newPagination = doc.querySelector(".pagination");

                if (newProducts && newProducts.children.length > 0) {
                    productsGrid.innerHTML = newProducts.innerHTML;
                    productsGrid.style.display = "grid";
                    noResultsDiv.style.display = "none";
                } else {
                    productsGrid.style.display = "none";
                    noResultsDiv.style.display = "block";
                    noResultsDiv.innerHTML = newNoResults?.innerHTML || "<p>No results</p>";
                }

                if (newPagination) {
                    paginationContainer.innerHTML = newPagination.innerHTML;
                }
            })
            .catch(err => console.error("Error fetching filtered products:", err));
    }

    categoryInputs.forEach(input => {
        input.addEventListener("change", () => {
            currentFilters.categoryId = input.value;
            currentFilters.page = 1;
            filterProducts();
        });
    });

    applyPriceBtn.addEventListener("click", () => {
        currentFilters.minPrice = minPriceInput.value;
        currentFilters.maxPrice = maxPriceInput.value;
        currentFilters.page = 1;
        filterProducts();
    });

    sortSelect.addEventListener("change", () => {
        currentFilters.sortBy = sortSelect.value;
        currentFilters.page = 1;
        filterProducts();
    });

    clearFiltersBtn.addEventListener("click", () => {
        currentFilters = {
            categoryId: '',
            minPrice: '',
            maxPrice: '',
            sortBy: 'default',
            page: 1
        };
        categoryInputs.forEach(radio => radio.checked = false);
        minPriceInput.value = '';
        maxPriceInput.value = '';
        sortSelect.value = 'default';
        filterProducts();
    });

    gridViewBtn.addEventListener("click", () => {
        productsGrid.classList.remove("list-view");
        gridViewBtn.classList.add("active");
        listViewBtn.classList.remove("active");
    });

    listViewBtn.addEventListener("click", () => {
        productsGrid.classList.add("list-view");
        listViewBtn.classList.add("active");
        gridViewBtn.classList.remove("active");
    });

    document.addEventListener("click", (e) => {
        if (e.target.tagName === 'A' && e.target.closest('.pagination-list')) {
            e.preventDefault();
            const page = parseInt(e.target.textContent);
            if (!isNaN(page)) {
                currentFilters.page = page;
                filterProducts();
            }
        }
    });

    filterProducts();
});
