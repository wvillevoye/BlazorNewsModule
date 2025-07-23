
function initializeDiscoverySearch(targetWindow = '_blank') {
    const searchForm = document.getElementById('discovery-search-form');
    const searchInput = document.getElementById('discovery-search');
    const urlBase = 'https://maastrichtuniversity.on.worldcat.org/external-search?queryString=#T#&clusterResults=on&groupVariantRecords=off&stickyFacetsChecked=on';

    if (!searchForm || !searchInput) return;

    searchForm.addEventListener('submit', function (e) {
        e.preventDefault();

        const query = searchInput.value.trim();
        if (query.length === 0) return;

        const finalUrl = urlBase.replace('#T#', encodeURIComponent(query));
        window.open(finalUrl, targetWindow);
    });
}

window.initializeDiscoverySearch = initializeDiscoverySearch;
