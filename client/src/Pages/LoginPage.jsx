import React, { useState, useEffect } from "react";
import axios from "axios";
import "bootstrap/dist/css/bootstrap.min.css";
import SHA256 from "crypto-js/sha256";
import { useNavigate } from "react-router-dom";
import logo from "../Images/logo.png";

function LoginPage() {
  const [isLogin, setIsLogin] = useState(true);
  const [kullanici, setKullanici] = useState("");
  const [sifre, setSifre] = useState("");
  const [error, setError] = useState("");
  const [message, setMessage] = useState("");
  const navigate = useNavigate();

  // token varsa ve geçerliyse /panel e yönlendir
  useEffect(() => {
    const token = localStorage.getItem("authToken");
    if (token) {
      axios
        .get("http://localhost:5281/api/Auth/verify-token", {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        })
        .then((response) => {
          if (response.data.message === "Token geçerli.") {
            navigate("/panel");
          }
        })
        .catch(() => {
          localStorage.removeItem("authToken");
        });
    }
  }, [navigate]);

  //üye olma ve giriş fonksiyonları
  const handleSubmit = async (e) => {
    e.preventDefault();
    const hashedPassword = SHA256(sifre).toString();

    const endpoint = isLogin
      ? "http://localhost:5281/api/Auth/login"
      : "http://localhost:5281/api/Auth/register";

    try {
      const response = await axios.post(
        endpoint,
        {
          userName: kullanici,
          password: hashedPassword,
          ...(isLogin ? {} : { createdAt: new Date().toISOString() }),
        },
        {
          headers: { "Content-Type": "application/json" },
        }
      );

      if (isLogin) {
        localStorage.setItem("authToken", response.data.token);
        navigate("/panel");
      } else {
        setMessage("Kayıt başarılı! Giriş yapabilirsiniz.");
        setIsLogin(true);
      }

      setError("");
    } catch (error) {
      if (error.response) {
        setError(error.response.data || "Bir hata oluştu.");
      } else {
        setError("Sunucuya bağlanılamadı.");
      }
    }
  };

  return (
    <div className="d-flex align-items-center justify-content-center min-vh-100 bg-light">
      <div
        className="card p-4 shadow-sm border-0"
        style={{ width: "100%", maxWidth: "400px" }}
      >
        <h3 className="text-center mb-1 fw-bold">
          {isLogin ? "Giriş" : "Üye Ol"}
        </h3>
        <img
          src={logo}
          style={{
            width: "250px",
            marginInline: "auto",
            marginBlock: "15px",
          }}
        />

        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label htmlFor="kullanici" className="form-label fw-semibold">
              Kullanıcı Adı
            </label>
            <input
              type="text"
              className="form-control"
              id="kullanici"
              placeholder="Kullanıcı"
              value={kullanici}
              onChange={(e) => setKullanici(e.target.value)}
              required
            />
          </div>

          <div className="mb-4">
            <label htmlFor="sifre" className="form-label fw-semibold">
              Şifre
            </label>
            <input
              type="password"
              className="form-control"
              id="sifre"
              placeholder="Şifre"
              value={sifre}
              onChange={(e) => setSifre(e.target.value)}
              required
            />
          </div>

          {error && <div className="alert alert-danger">{error}</div>}
          {message && <div className="alert alert-success">{message}</div>}

          <button type="submit" className="btn btn-primary w-100 fw-semibold">
            {isLogin ? "Giriş Yap" : "Kayıt Ol"}
          </button>
        </form>

        <div className="text-center mt-3">
          <small>
            {isLogin ? "Hesabınız yok mu? " : "Zaten bir hesabınız var mı? "}
            <button
              type="button"
              onClick={() => {
                setIsLogin(!isLogin);
                setError("");
                setMessage("");
              }}
              className="btn btn-link p-0 align-baseline"
            >
              {isLogin ? "Kayıt Ol" : "Giriş Yap"}
            </button>
          </small>
        </div>
      </div>
    </div>
  );
}

export default LoginPage;
