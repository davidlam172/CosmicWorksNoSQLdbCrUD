async function fetchData(token) {
    const apiUrl = 'https://cosmicworkstest220230929010127.azurewebsites.net/addproduct'; // Replace with your actual API endpoint

    try {
        const response = await fetch(apiUrl, {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json'
                // Add other headers if needed
            }
        });

        if (!response.ok) {
            throw new Error(`Request failed with status ${response.status}`);
        }

        const data = await response.json();
        console.log('Data received:', data);
        // Process the data as needed

    } catch (error) {
        console.error('Error fetching data:', error);
        // Handle errors appropriately
    }
}
