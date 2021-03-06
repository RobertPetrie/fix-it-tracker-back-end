﻿using fix_it_tracker_back_end.Controllers;
using fix_it_tracker_back_end.Data.Repositories;
using fix_it_tracker_back_end.Dtos;
using fix_it_tracker_back_end.Model;
using fix_it_tracker_back_end.Model.BindingTargets;
using fix_it_tracker_back_end_unit_tests.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace fix_it_tracker_back_end_unit_tests
{
    public class ItemTypeControllerTest
    {
        private ItemTypeController _itemTypeController;
        private IFixItTrackerRepository _fixItTrackerRepository;

        private static int EXISTING_ITEM_TYPE_ID = 1;
        private static int NON_EXISTING_ITEM_TYPE_ID = 25;
        private static int NUM_OF_ITEM_TYPE = 5;

        public ItemTypeControllerTest()
        {
            _fixItTrackerRepository = new UnitTestsRepository();
            _itemTypeController = new ItemTypeController(_fixItTrackerRepository, UnitTestsMapping.GetMapper());
        }

        // GET api/itemtype
        [Fact]
        public void GetItemTypes_ReturnsOkResult()
        {
            var okResult = _itemTypeController.GetItemTypes();
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetItemTypes_ReturnsRightItem()
        {
            var okResult = _itemTypeController.GetItemTypes().Result as OkObjectResult;
            Assert.Equal(EXISTING_ITEM_TYPE_ID, (okResult.Value as List<ItemTypeGetDto>).FirstOrDefault(i => i.ItemTypeID == EXISTING_ITEM_TYPE_ID).ItemTypeID);
        }

        [Fact]
        public void GetItemTypes_ReturnsAllItems()
        {
            var okResult = _itemTypeController.GetItemTypes().Result as OkObjectResult;
            var items = Assert.IsType<List<ItemTypeGetDto>>(okResult.Value);
            Assert.Equal(NUM_OF_ITEM_TYPE, items.Count);
        }

        [Fact]
        public void GetItemTypes_ReturnsNotFound()
        {
            _fixItTrackerRepository = new UnitTestsRepository(noItemTypes: true);
            _itemTypeController = new ItemTypeController(_fixItTrackerRepository, UnitTestsMapping.GetMapper());

            var okResult = _itemTypeController.GetItemTypes();
            Assert.IsType<NotFoundObjectResult>(okResult.Result);
        }

        // GET api/itemtype/5
        [Fact]
        public void GetItemType_ReturnsOkResult()
        {
            var okResult = _itemTypeController.GetItemType(EXISTING_ITEM_TYPE_ID);
            Assert.IsType<OkObjectResult>(okResult.Result);
        }

        [Fact]
        public void GetItemType_ReturnsRightItem()
        {
            var okResult = _itemTypeController.GetItemType(EXISTING_ITEM_TYPE_ID).Result as OkObjectResult;
            Assert.IsType<ItemTypeGetDto>(okResult.Value);
            Assert.Equal(EXISTING_ITEM_TYPE_ID, (okResult.Value as ItemTypeGetDto).ItemTypeID);
        }

        [Fact]
        public void GetItemType_ReturnsNotFound()
        {
            var okResult = _itemTypeController.GetItemType(NON_EXISTING_ITEM_TYPE_ID);
            Assert.IsType<NotFoundObjectResult>(okResult.Result);
        }

        // POST api/itemtype/
        [Fact]
        public void AddItemType_ReturnsCreatedResponse()
        {
            ItemTypeData itemType = new ItemTypeData
            {
                Name = "Test Name",
                Model = "Test Model",
                Manufacturer = "Test Manufacturer"
            };

            var createdResponse = _itemTypeController.CreateItemType(itemType);

            Assert.IsType<CreatedResult>(createdResponse);
        }

        [Fact]
        public void AddItemType_ReturnedResponseHasCreatedMessage()
        {
            ItemTypeData itemType = new ItemTypeData
            {
                Name = "Test Name",
                Model = "Test Model",
                Manufacturer = "Test Manufacturer"
            };

            ActionResult<ItemType> actionResult = _itemTypeController.CreateItemType(itemType);
            CreatedResult createdResult = actionResult.Result as CreatedResult;
            var result = createdResult.Value;

            Assert.Equal("Item Type Created", result);
        }

        [Fact]
        public void AddItemType_ReturnsBadRequest()
        {
            ItemTypeData itemType = new ItemTypeData();

            _itemTypeController.ModelState.AddModelError("Name", "Required");

            var badResponse = _itemTypeController.CreateItemType(itemType);

            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public void AddItemType_ExistingitemTypeReturnsBadRequest()
        {
            ItemTypeData firstItemType = new ItemTypeData
            {
                Name = "Test Name",
                Model = "Test Model",
                Manufacturer = "Test Manufacturer"
            };

            ItemTypeData secondItemType = new ItemTypeData
            {
                Name = "Test Name",
                Model = "Test Model",
                Manufacturer = "Test Manufacturer"
            };

            _itemTypeController.CreateItemType(firstItemType);

            var badResponse = _itemTypeController.CreateItemType(secondItemType);

            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        // PUT api/itemtype/
        [Fact]
        public void ReplaceItemType_ReturnsOkResult()
        {
            ItemTypeData itemType = new ItemTypeData
            {
                Name = "Test Name",
                Model = "Test Model",
                Manufacturer = "Test Manufacturer"
            };

            var okResult = _itemTypeController.ReplaceItemType(1, itemType);

            Assert.IsType<OkObjectResult>(okResult);
        }

        [Fact]
        public void ReplaceItemType_ReturnedResponseHasResponseMessage()
        {
            ItemTypeData itemType = new ItemTypeData
            {
                Name = "Test Name",
                Model = "Test Model",
                Manufacturer = "Test Manufacturer"
            };

            ActionResult<ItemTypeData> actionResult = _itemTypeController.ReplaceItemType(1, itemType);
            OkObjectResult createdResult = actionResult.Result as OkObjectResult;
            var result = createdResult.Value;

            Assert.Equal("The Item Type has been updated.", result);
        }

        [Fact]
        public void ReplaceItemType_ReturnsBadRequest()
        {
            ItemTypeData itemType = new ItemTypeData();

            _itemTypeController.ModelState.AddModelError("Name", "Required");

            var badResponse = _itemTypeController.ReplaceItemType(1, itemType);

            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

        [Fact]
        public void ReplaceItemType_NonExistingitemTypeReturnsBadRequest()
        {
            ItemTypeData itemType = new ItemTypeData
            {
                Name = "Test Name",
                Model = "Test Model",
                Manufacturer = "Test Manufacturer"
            };

            var badResponse = _itemTypeController.ReplaceItemType(1234, itemType);

            Assert.IsType<BadRequestObjectResult>(badResponse);
        }

    }
}
