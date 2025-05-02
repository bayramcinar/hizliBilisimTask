import { useNavigate } from "react-router-dom";

function LogoutButton() {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("authToken"); // Token'ı sil
    navigate("/login"); // Login sayfasına yönlendir
  };

  return (
    <button
      className="btn btn-danger"
      style={{ position: "absolute", top: 10, right: 10 }}
      onClick={handleLogout}
    >
      Çıkış Yap
    </button>
  );
}

export default LogoutButton;
