using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Moq;
using NUnit.Framework;
using PhotoAlbum.BLL.EnittyBLL;
using PhotoAlbum.BLL.Services;
using PhotoAlbum.DAL.Entities;
using PhotoAlbum.DAL.Interfaces;

namespace PhotoAlbum.UnitTests
{
    [TestFixture]
    public class PhotoServiceTest
    {

        [Test]
        public void AddPhoto_PassValidData_CallsAddAndSaves()
        {
            var photoBll = new UserPhotoBLL();
            var unitOfWork = new Mock<IPhotoUnitOfWork>();

            var userProfile = new UserProfile
            {
                Photos = new List<UserPhoto> { new UserPhoto() }
            };

            unitOfWork.Setup(u => u.UserRepository.Get(It.IsAny<string>()))
                .Returns(userProfile);
            unitOfWork.Setup(u => u.UserRepository.Find(It.IsAny<Expression<Func<UserProfile,bool>>>()))
                .Returns(new List<UserProfile>{new UserProfile()});
            unitOfWork.Setup(u => u.Photos.Add(It.IsAny<UserPhoto>()))
                .Callback<UserPhoto>(t => userProfile.Photos.Add(t));


            // Act

            var photoService = new PhotoService(unitOfWork.Object);
            photoService.AddPhoto(photoBll);

            // Assert

            unitOfWork.Verify(u => u.Photos.Add(It.IsAny<UserPhoto>()), Times.Once());
            unitOfWork.Verify(u => u.SaveAsync(), Times.Once());
            Assert.IsTrue(userProfile.Photos.Count == 2);
        }
    }
}
