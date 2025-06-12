import { useNavigate } from "react-router-dom";

const Header = () => {
  const navigate = useNavigate();
  const handleLogoutClick = () => {
    localStorage.removeItem("TOKEN_KEY");
    navigate("/");
  };
  return (
    <header className="bg-dark text-white p-4">
      <div className="container mx-auto d-flex justify-content-between align-items-center">
        <div className="d-flex">
          <h1 className="text-2xl font-bold" onClick={() => navigate('/')} role="button">Music App</h1>
        </div>

        <button className="btn btn-danger" onClick={handleLogoutClick}>
          Logout
        </button>
      </div>
    </header>
  );
};

export default Header;
