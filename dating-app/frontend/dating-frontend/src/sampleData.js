// Sample user data with attractive photos
export const sampleUsers = [
  {
    id: 1,
    email: "sarah@example.com",
    name: "Sarah",
    age: 25,
    bio: "Love hiking, yoga, and good coffee ☕ Looking for someone to explore the city with!",
    location: "New York, NY",
    gender: "Female",
    interestedIn: "Male",
    photos: [
      "https://images.unsplash.com/photo-1494790108755-2616b612b786?w=400&h=600&fit=crop&crop=face",
      "https://images.unsplash.com/photo-1517841905240-472988babdf9?w=400&h=600&fit=crop&crop=face"
    ]
  },
  {
    id: 2,
    email: "mike@example.com",
    name: "Mike",
    age: 28,
    bio: "Fitness enthusiast 💪 Love cooking and weekend adventures. Let's grab dinner!",
    location: "Los Angeles, CA",
    gender: "Male",
    interestedIn: "Female",
    photos: [
      "https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=400&h=600&fit=crop&crop=face",
      "https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=400&h=600&fit=crop&crop=face"
    ]
  },
  {
    id: 3,
    email: "emma@example.com",
    name: "Emma",
    age: 23,
    bio: "Artist and dog lover 🎨🐕 Always up for museum visits and long walks in the park",
    location: "Chicago, IL",
    gender: "Female",
    interestedIn: "Male",
    photos: [
      "https://images.unsplash.com/photo-1438761681033-6461ffad8d80?w=400&h=600&fit=crop&crop=face",
      "https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=400&h=600&fit=crop&crop=face"
    ]
  },
  {
    id: 4,
    email: "alex@example.com",
    name: "Alex",
    age: 30,
    bio: "Tech entrepreneur who loves travel ✈️ Seeking someone who shares my passion for life",
    location: "San Francisco, CA",
    gender: "Male",
    interestedIn: "Female",
    photos: [
      "https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?w=400&h=600&fit=crop&crop=face",
      "https://images.unsplash.com/photo-1519085360753-af0119f7cbe7?w=400&h=600&fit=crop&crop=face"
    ]
  },
  {
    id: 5,
    email: "jessica@example.com",
    name: "Jessica",
    age: 26,
    bio: "Photographer and foodie 📸🍕 Love capturing beautiful moments and trying new cuisines",
    location: "Miami, FL",
    gender: "Female",
    interestedIn: "Male",
    photos: [
      "https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=400&h=600&fit=crop&crop=face",
      "https://images.unsplash.com/photo-1506863530036-1efeddceb993?w=400&h=600&fit=crop&crop=face"
    ]
  },
  {
    id: 6,
    email: "david@example.com",
    name: "David",
    age: 27,
    bio: "Musician and coffee addict ☕🎸 Looking for someone to share concerts and cozy evenings",
    location: "Austin, TX",
    gender: "Male",
    interestedIn: "Female",
    photos: [
      "https://images.unsplash.com/photo-1506794778202-cad84cf45f1d?w=400&h=600&fit=crop&crop=face",
      "https://images.unsplash.com/photo-1507591064344-4c6ce005b128?w=400&h=600&fit=crop&crop=face"
    ]
  },
  {
    id: 7,
    email: "olivia@example.com",
    name: "Olivia",
    age: 24,
    bio: "Yoga instructor and nature lover 🧘‍♀️🌿 Seeking mindful connections and outdoor adventures",
    location: "Denver, CO",
    gender: "Female",
    interestedIn: "Male",
    photos: [
      "https://images.unsplash.com/photo-1524504388940-b1c1722653e1?w=400&h=600&fit=crop&crop=face",
      "https://images.unsplash.com/photo-1487412720507-e7ab37603c6f?w=400&h=600&fit=crop&crop=face"
    ]
  },
  {
    id: 8,
    email: "ryan@example.com",
    name: "Ryan",
    age: 29,
    bio: "Chef and wine enthusiast 🍷👨‍🍳 Let me cook for you while we talk about our dreams",
    location: "Seattle, WA",
    gender: "Male",
    interestedIn: "Female",
    photos: [
      "https://images.unsplash.com/photo-1492562080023-ab3db95bfbce?w=400&h=600&fit=crop&crop=face",
      "https://images.unsplash.com/photo-1521119989659-a83eee488004?w=400&h=600&fit=crop&crop=face"
    ]
  }
];

// Function to initialize sample data in backend
export const initializeSampleData = async () => {
  try {
    // Create sample users
    for (const user of sampleUsers) {
      const response = await fetch('http://localhost:5001/api/users', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          email: user.email,
          name: user.name,
          age: user.age,
          bio: user.bio,
          location: user.location,
          gender: user.gender,
          interestedIn: user.interestedIn
        })
      });
      
      if (response.ok) {
        const createdUser = await response.json();
        
        // Add photos to user
        for (const photoUrl of user.photos) {
          await fetch(`http://localhost:5001/api/users/${createdUser.id}/photos`, {
            method: 'POST',
            headers: {
              'Content-Type': 'application/json',
            },
            body: JSON.stringify(photoUrl)
          });
        }
      }
    }
    
    // Sync users with match service
    const usersResponse = await fetch('http://localhost:5001/api/users');
    const users = await usersResponse.json();
    
    await fetch('http://localhost:5003/api/match/sync-users', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(users)
    });
    
    console.log('Sample data initialized successfully!');
  } catch (error) {
    console.error('Error initializing sample data:', error);
  }
};