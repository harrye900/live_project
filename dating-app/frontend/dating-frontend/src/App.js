import React, { useState, useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate, useNavigate } from 'react-router-dom';
import axios from 'axios';
import { initializeSampleData } from './sampleData';
import './App.css';

const API_BASE = 'http://localhost:5001/api';

function App() {
  const [currentUser, setCurrentUser] = useState(null);
  const [dataInitialized, setDataInitialized] = useState(false);

  useEffect(() => {
    const initData = async () => {
      if (!dataInitialized) {
        await initializeSampleData();
        setDataInitialized(true);
      }
    };
    initData();
  }, [dataInitialized]);

  return (
    <Router>
      <div className="App">
        <div>
        <Routes>
          <Route path="/" element={currentUser ? <Navigate to="/dashboard" replace /> : <LandingPage />} />
          <Route path="/login" element={<Login setCurrentUser={setCurrentUser} />} />
          <Route path="/register" element={currentUser ? <Navigate to="/dashboard" replace /> : <Register setCurrentUser={setCurrentUser} />} />
          <Route path="/dashboard" element={currentUser ? <Dashboard user={currentUser} /> : <Navigate to="/" />} />
          <Route path="/profile" element={currentUser ? <Profile user={currentUser} setCurrentUser={setCurrentUser} /> : <Navigate to="/" />} />
          <Route path="/matches" element={currentUser ? <Matches user={currentUser} /> : <Navigate to="/" />} />
        </Routes>
        </div>
      </div>
    </Router>
  );
}

function LandingPage() {
  return (
    <div className="auth-container">
      <div className="logo">
        <h1>❤️CatchLove</h1>
        <p>Where Hearts Connect</p>
      </div>
      <div className="landing-buttons">
        <a href="/login" className="auth-btn login-btn">Login</a>
        <a href="/register" className="auth-btn register-btn">Register</a>
      </div>
      <div className="stats-section">
        <h3>120,000+ NEW MEMBERS</h3>
        <p>per month, total number of global registrations</p>
      </div>
      <div className="safety-section">
        <h3>SAFE & SECURE</h3>
        <p>Daily profile reviews help maintain a safe, genuine, and trustworthy dating community</p>
      </div>
    </div>
  );
}

function Login({ setCurrentUser }) {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const navigate = useNavigate();

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(`${API_BASE}/users/login`, {
        email,
        password
      });
      console.log('Login response:', response.data);
      setCurrentUser(response.data);
      navigate('/profile');
    } catch (error) {
      console.error('Login error:', error);
      alert('Invalid email or password');
    }
  };

  return (
    <div className="auth-container">
      <div className="logo">
        <h1>❤️CatchLove</h1>
        <p>Welcome Back</p>
      </div>
      <form onSubmit={handleLogin}>
        <input
          type="email"
          placeholder="Email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
          autoComplete="off"
          required
        />
        <input
          type="password"
          placeholder="Password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
          autoComplete="new-password"
          required
        />
        <button type="submit">Login</button>
      </form>
      <p>Don't have an account? <a href="/register">Register</a></p>

    </div>
  );
}

function Register({ setCurrentUser }) {
  const [step, setStep] = useState(1);
  const [formData, setFormData] = useState({
    email: '',
    password: '',
    name: '',
    age: '',
    bio: '',
    location: '',
    zipcode: '',
    gender: '',
    interestedIn: ''
  });
  const [photos, setPhotos] = useState([]);
  const [newUser, setNewUser] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      const response = await axios.post(`${API_BASE}/users`, {
        ...formData,
        age: parseInt(formData.age)
      });
      setNewUser(response.data);
      setStep(2);
    } catch (error) {
      console.error('Registration error:', error);
    }
  };

  const handlePhotoUpload = (e) => {
    const files = Array.from(e.target.files);
    if (photos.length + files.length <= 5) {
      const newPhotos = files.map(file => URL.createObjectURL(file));
      setPhotos([...photos, ...newPhotos]);
    } else {
      alert('Maximum 5 photos allowed');
    }
  };

  const removePhoto = (index) => {
    setPhotos(photos.filter((_, i) => i !== index));
  };

  const completeRegistration = () => {
    console.log('Completing registration with user:', newUser);
    console.log('Photos:', photos);
    const userWithPhotos = { ...newUser, photos };
    console.log('Final user object:', userWithPhotos);
    setCurrentUser(userWithPhotos);
  };

  if (step === 1) {
    return (
      <div className="auth-container">
        <div className="logo">
          <h1>❤️CatchLove</h1>
          <p>Join Today</p>
        </div>
        <form onSubmit={handleSubmit}>
          <input
            type="email"
            placeholder="Email"
            value={formData.email}
            onChange={(e) => setFormData({...formData, email: e.target.value})}
            autoComplete="off"
            required
          />
          <input
            type="password"
            placeholder="Password"
            value={formData.password}
            onChange={(e) => setFormData({...formData, password: e.target.value})}
            autoComplete="new-password"
            required
          />
          <input
            type="text"
            placeholder="Name"
            value={formData.name}
            onChange={(e) => setFormData({...formData, name: e.target.value})}
            required
          />
          <input
            type="number"
            placeholder="Age"
            value={formData.age}
            onChange={(e) => setFormData({...formData, age: e.target.value})}
            required
          />
          <textarea
            placeholder="Bio"
            value={formData.bio}
            onChange={(e) => setFormData({...formData, bio: e.target.value})}
          />
          <input
            type="text"
            placeholder="Location"
            value={formData.location}
            onChange={(e) => setFormData({...formData, location: e.target.value})}
          />
          <input
            type="text"
            placeholder="Zipcode"
            value={formData.zipcode}
            onChange={(e) => setFormData({...formData, zipcode: e.target.value})}
          />
          <select
            value={formData.gender}
            onChange={(e) => setFormData({...formData, gender: e.target.value})}
            required
          >
            <option value="">Select Gender</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
            <option value="Other">Other</option>
          </select>
          <select
            value={formData.interestedIn}
            onChange={(e) => setFormData({...formData, interestedIn: e.target.value})}
            required
          >
            <option value="">Interested In</option>
            <option value="Male">Male</option>
            <option value="Female">Female</option>
            <option value="Other">Other</option>
          </select>
          <button type="submit">Next: Add Photos</button>
        </form>
        <p>Already have an account? <a href="/">Login</a></p>
      </div>
    );
  }

  return (
    <div className="auth-container">
      <div className="logo">
        <h1>❤️CatchLove</h1>
        <p>Add Your Photos ({photos.length}/5)</p>
      </div>
      <div className="photo-upload-section">
        <div className="photo-grid">
          {photos.map((photo, index) => (
            <div key={index} className="photo-preview">
              <img src={photo} alt={`Preview ${index + 1}`} />
              <button type="button" onClick={() => removePhoto(index)} className="remove-photo">×</button>
            </div>
          ))}
          {photos.length < 5 && (
            <label className="photo-upload-btn">
              <input
                type="file"
                accept="image/*"
                multiple
                onChange={handlePhotoUpload}
                style={{ display: 'none' }}
              />
              <span>+</span>
              <p>Add Photo</p>
            </label>
          )}
        </div>
        <div className="upload-actions">
          <button onClick={completeRegistration} className="complete-btn">
            {photos.length > 0 ? 'Complete Profile' : 'Skip Photos'}
          </button>
        </div>
      </div>
    </div>
  );
}

function Dashboard({ user }) {
  const [potentialMatches, setPotentialMatches] = useState([]);
  const [currentIndex, setCurrentIndex] = useState(0);
  const [currentPhotoIndex, setCurrentPhotoIndex] = useState(0);

  useEffect(() => {
    fetchPotentialMatches();
  }, []);

  const fetchPotentialMatches = async () => {
    try {
      const response = await axios.get(`http://localhost:5003/api/match/potential/${user.id}`);
      setPotentialMatches(response.data);
    } catch (error) {
      console.error('Error fetching matches:', error);
    }
  };

  const handleSwipe = async (isLike) => {
    if (currentIndex >= potentialMatches.length) return;

    try {
      const response = await axios.post('http://localhost:5003/api/match/swipe', {
        userId: user.id,
        targetUserId: potentialMatches[currentIndex].id,
        isLike
      });

      if (response.data.isMatch) {
        alert('It\'s a match! 🎉');
      }

      setCurrentIndex(currentIndex + 1);
      setCurrentPhotoIndex(0);
    } catch (error) {
      console.error('Swipe error:', error);
    }
  };

  const nextPhoto = () => {
    const currentMatch = potentialMatches[currentIndex];
    if (currentMatch && currentPhotoIndex < currentMatch.photos.length - 1) {
      setCurrentPhotoIndex(currentPhotoIndex + 1);
    }
  };

  const prevPhoto = () => {
    if (currentPhotoIndex > 0) {
      setCurrentPhotoIndex(currentPhotoIndex - 1);
    }
  };

  const currentMatch = potentialMatches[currentIndex];

  return (
    <div className="dashboard">
      <nav>
        <h1>❤️CatchLove</h1>
        <div>
          <a href="/profile">Profile</a>
          <a href="/matches">Matches</a>
        </div>
      </nav>
      
      {currentMatch ? (
        <div className="card-container">
          <div className="profile-card">
            <div className="photo-container">
              {currentMatch.photos.length > 0 ? (
                <>
                  <img 
                    src={currentMatch.photos[currentPhotoIndex]} 
                    alt={currentMatch.name} 
                    className="main-photo"
                    onClick={nextPhoto}
                  />
                  <div className="photo-indicators">
                    {currentMatch.photos.map((_, index) => (
                      <div 
                        key={index} 
                        className={`indicator ${index === currentPhotoIndex ? 'active' : ''}`}
                        onClick={() => setCurrentPhotoIndex(index)}
                      ></div>
                    ))}
                  </div>
                  {currentMatch.photos.length > 1 && (
                    <>
                      <button className="photo-nav prev" onClick={prevPhoto}>‹</button>
                      <button className="photo-nav next" onClick={nextPhoto}>›</button>
                    </>
                  )}
                </>
              ) : (
                <div className="no-photo">No Photo</div>
              )}
            </div>
            <div className="profile-info">
              <h2>{currentMatch.name}, {currentMatch.age}</h2>
              <p className="bio">{currentMatch.bio}</p>
              <p className="location">📍 {currentMatch.location}</p>
            </div>
          </div>
          <div className="action-buttons">
            <button className="pass-btn" onClick={() => handleSwipe(false)}>
              <span className="btn-icon">✕</span>
              <span className="btn-text">Pass</span>
            </button>
            <button className="like-btn" onClick={() => handleSwipe(true)}>
              <span className="btn-icon">♥</span>
              <span className="btn-text">Like</span>
            </button>
          </div>
        </div>
      ) : (
        <div className="no-more-matches">
          <h2>No more potential matches!</h2>
          <p>Check back later for new profiles.</p>
        </div>
      )}
    </div>
  );
}

function Profile({ user, setCurrentUser }) {
  const [photos, setPhotos] = useState([]);
  const [selectedFile, setSelectedFile] = useState(null);

  useEffect(() => {
    fetchPhotos();
  }, []);

  const fetchPhotos = async () => {
    try {
      const response = await axios.get(`http://localhost:5002/api/photos/user/${user.id}`);
      setPhotos(response.data);
    } catch (error) {
      console.error('Error fetching photos:', error);
    }
  };

  const handlePhotoUpload = async (e) => {
    e.preventDefault();
    if (!selectedFile) return;

    const formData = new FormData();
    formData.append('file', selectedFile);
    formData.append('userId', user.id);
    formData.append('isMain', photos.length === 0);

    try {
      await axios.post('http://localhost:5002/api/photos/upload', formData, {
        headers: { 'Content-Type': 'multipart/form-data' }
      });
      fetchPhotos();
      setSelectedFile(null);
    } catch (error) {
      console.error('Upload error:', error);
    }
  };

  return (
    <div className="profile-page">
      <nav>
        <h1>Your Profile</h1>
        <a href="/dashboard">Back to Dashboard</a>
      </nav>
      
      <div className="profile-content">
        <div className="profile-info">
          <h2>{user.name}, {user.age}</h2>
          <p>{user.bio}</p>
          <p>📍 {user.location}</p>
          <p>📮 {user.zipcode}</p>
          <p>Gender: {user.gender}</p>
          <p>Looking for: {user.interestedIn}</p>
        </div>

        <div className="photo-section">
          <h3>Your Photos ({user.photos?.length || 0})</h3>
          <div className="photo-grid">
            {user.photos?.map((photo, index) => (
              <div key={index} className="photo-item">
                <img src={photo} alt="Profile" />
              </div>
            ))}
          </div>
          
          <form onSubmit={handlePhotoUpload} className="upload-form">
            <input
              type="file"
              accept="image/*"
              onChange={(e) => setSelectedFile(e.target.files[0])}
            />
            <button type="submit">Upload Photo</button>
          </form>
        </div>
      </div>
    </div>
  );
}

function Matches({ user }) {
  const [matches, setMatches] = useState([]);

  useEffect(() => {
    fetchMatches();
  }, []);

  const fetchMatches = async () => {
    try {
      const response = await axios.get(`http://localhost:5003/api/match/matches/${user.id}`);
      setMatches(response.data);
    } catch (error) {
      console.error('Error fetching matches:', error);
    }
  };

  return (
    <div className="matches-page">
      <nav>
        <h1>Your Matches ({matches.length})</h1>
        <a href="/dashboard">Back to Dashboard</a>
      </nav>
      
      <div className="matches-grid">
        {matches.length > 0 ? (
          matches.map(match => (
            <div key={match.matchId} className="match-card">
              <div className="match-photo">
                {match.matchedUser?.photos?.length > 0 ? (
                  <img src={match.matchedUser.photos[0]} alt={match.matchedUser.name} />
                ) : (
                  <div className="no-photo">No Photo</div>
                )}
              </div>
              <h3>{match.matchedUser?.name}</h3>
              <p>Matched on {new Date(match.matchedAt).toLocaleDateString()}</p>
            </div>
          ))
        ) : (
          <div className="no-matches">
            <h3>No matches yet</h3>
            <p>Keep swiping to find your perfect match! 💕</p>
          </div>
        )}
      </div>
    </div>
  );
}

export default App;