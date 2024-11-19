function fetchUserData() {
    fetch('http://localhost:8080/api/users')
      .then(response => {
        if (!response.ok) {
          throw new Error('Network response was not ok ' + response.statusText);
        }
        return response.json(); // Parse the JSON response
      })
      .then(data => {
        // Display the data in a div with ID 'user-list'
        const userList = document.getElementById('user-list');
        if (userList) {
          userList.innerHTML = ''; // Clear any existing content
          data.forEach(user => {
            // Create an element for each user
            const userItem = document.createElement('div');
            userItem.textContent = `${user.id}: ${user.username} (${user.password})`;
            userList.appendChild(userItem);
          });
        }
      })
      .catch(error => {
        console.error('Error fetching user data:', error);
      });
  }
  
  // Attach event listener to the button
  document.getElementById('fetch-users-button').addEventListener('click', fetchUserData);